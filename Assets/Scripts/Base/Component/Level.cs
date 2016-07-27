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
        levelState.remainingBubbles--;
        levelState.NotifyListeners();
    }

    private void OnBubbleDestroyed(BubbleDestroyedEvent gameEvent)
    {
        levelState.UpdateTypeTotals(gameEvent.bubble.GetComponent<BubbleAttachments>().Model.type, -1);
        levelState.score += gameEvent.score;
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

        UpdateUserScoreAndStars();

        var stars = GlobalState.Instance.Services.Get<UserStateService>().levels[levelState.levelNumber].stars;

        GlobalState.Instance.Services.Get<PopupService>().Enqueue(new GenericPopupConfig
        {
            title = "Level Won",
            mainText = ("Score: " + levelState.score.ToString() + "\n" +
                        "Stars: " + stars.ToString()),
            closeActions = new List<Action> { DispatchLevelWon },
            affirmativeActions = new List<Action> { DispatchLevelWon }
        });
    }

    private void UpdateUserScoreAndStars()
    {
        var user = GlobalState.Instance.Services.Get<UserStateService>();

        if (user.levels[levelState.levelNumber].score < levelState.score)
        {
            user.levels[levelState.levelNumber].score = levelState.score;

            for (int starIndex = levelState.starValues.Length - 1; starIndex >= 0; starIndex--)
            {
                if (levelState.score >= levelState.starValues[starIndex])
                {
                    user.levels[levelState.levelNumber].stars = (starIndex + 1);
                    break;
                }
            }
        }
    }

    private void DispatchLevelWon()
    {
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new LevelCompleteEvent(true));
    }
}
