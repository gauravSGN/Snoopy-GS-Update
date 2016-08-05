using System;
using Service;
using UI.Popup;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public readonly LevelState levelState = new LevelState();
    public BubbleFactory bubbleFactory;

    [SerializeField]
    private string levelAssetPath;

    [SerializeField]
    private LevelLoader loader;

    [SerializeField]
    private SpriteRenderer background;

    private string levelData;

    protected void Start()
    {
        var sceneData = GlobalState.Instance.Services.Get<SceneService>();

        if (!string.IsNullOrEmpty(sceneData.NextLevelData))
        {
            levelData = sceneData.NextLevelData;
            sceneData.NextLevelData = null;
        }
        else if (levelAssetPath != null)
        {
            levelData = GlobalState.Instance.Services.Get<AssetService>().LoadAsset<TextAsset>(levelAssetPath).text;
        }

        levelState.levelNumber = sceneData.LevelNumber;
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        loader.LoadLevel(levelData);
        levelState.typeTotals = loader.Configuration.Counts;
        levelState.score = 0;
        levelState.initialShotCount = loader.LevelData.ShotCount;
        levelState.remainingBubbles = loader.LevelData.ShotCount;
        levelState.starValues = loader.LevelData.StarValues;

        var bubbleQueue = BubbleQueueFactory.GetBubbleQueue(loader.BubbleQueueType, levelState, loader.LevelData.Queue);
        levelState.bubbleQueue = bubbleQueue;

        levelState.NotifyListeners();

        var eventService = GlobalState.Instance.Services.Get<EventService>();

        eventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        eventService.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        eventService.AddEventHandler<GoalCompleteEvent>(OnGoalComplete);

        var assetService = GlobalState.Instance.Services.Get<AssetService>();

        assetService.LoadAssetAsync<Sprite>(loader.LevelData.Background, delegate(Sprite sprite)
            {
                background.sprite = sprite;
            });

        assetService.OnComplete += OnAssetLoadingComplete;
    }

    private void OnAssetLoadingComplete()
    {
        var assetService = GlobalState.Instance.Services.Get<AssetService>();
        var eventService = GlobalState.Instance.Services.Get<EventService>();

        assetService.OnComplete -= OnAssetLoadingComplete;
        eventService.Dispatch(new LevelLoadedEvent());
    }

    private void OnBubbleFired(BubbleFiredEvent gameEvent)
    {
        levelState.UpdateTypeTotals(gameEvent.bubble.GetComponent<BubbleAttachments>().Model.type, 1);
        levelState.DecrementRemainingBubbles();
    }

    private void OnBubbleDestroyed(BubbleDestroyedEvent gameEvent)
    {
        levelState.UpdateTypeTotals(gameEvent.bubble.GetComponent<BubbleAttachments>().Model.type, -1);
        levelState.NotifyListeners();
    }

    private void OnGoalComplete(GoalCompleteEvent gameEvent)
    {
        foreach (var goal in loader.LevelData.goals)
        {
            if (!goal.Complete)
            {
                return;
            }
        }

        GlobalState.Instance.Services.Get<EventService>().Dispatch(new LevelCompleteEvent(true));
        UpdateUserScoreAndStars();

        var stars = GlobalState.Instance.Services.Get<UserStateService>().levels[levelState.levelNumber].stars;

        GlobalState.Instance.Services.Get<PopupService>().Enqueue(new GenericPopupConfig
        {
            title = "Level Won",
            mainText = ("Score: " + levelState.score.ToString() + "\n" +
                        "Stars: " + stars.ToString()),
            closeActions = new List<Action> { DispatchReturnToMap },
            affirmativeActions = new List<Action> { DispatchReturnToMap }
        });
    }

    private void UpdateUserScoreAndStars()
    {
        var user = GlobalState.User;
        var highScore = Math.Max(levelState.score, user.levels[levelState.levelNumber].score);

        // Only set data if we have to so we can avoid dispatching extra calls to GS
        if (user.levels[levelState.levelNumber].score < highScore)
        {
            user.levels[levelState.levelNumber].score = highScore;
        }

        for (int starIndex = levelState.starValues.Length - 1; starIndex >= 0; starIndex--)
        {
            if (highScore >= levelState.starValues[starIndex])
            {
                var newStars = (starIndex + 1);

                if (user.levels[levelState.levelNumber].stars < newStars)
                {
                    user.levels[levelState.levelNumber].stars = newStars;
                }

                break;
            }
        }
    }

    private void DispatchReturnToMap()
    {
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new ReturnToMapEvent());
    }
}
