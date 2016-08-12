using Util;
using Model;
using System;
using Service;
using UI.Popup;
using Modifiers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    private class ModifierFactory : AttributeDrivenFactory<LevelModifier, LevelModifierAttribute, LevelModifierType>
    {
        override protected LevelModifierType GetKeyFromAttribute(LevelModifierAttribute attribute)
        {
            return attribute.ModifierType;
        }
    }

    public readonly LevelState levelState = new LevelState();
    public BubbleFactory bubbleFactory;

    [SerializeField]
    private string levelAssetPath;

    [SerializeField]
    private LevelLoader loader;

    [SerializeField]
    private SpriteRenderer background;

    private readonly List<LevelModifier> modifiers = new List<LevelModifier>();
    private readonly ModifierFactory modifierFactory = new ModifierFactory();
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

        if (loader.LevelData.Modifiers != null)
        {
            foreach (var modifier in loader.LevelData.Modifiers)
            {
                AddModifier(modifier.Type, modifier.Data);
            }
        }

        var eventService = GlobalState.EventService;

        eventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        eventService.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        eventService.AddEventHandler<GoalCompleteEvent>(OnGoalComplete);
        eventService.AddEventHandler<AddLevelModifierEvent>(e => AddModifier(e.type, e.data));

        var assetService = GlobalState.Instance.Services.Get<AssetService>();

        assetService.LoadAssetAsync<Sprite>(loader.LevelData.Background, delegate(Sprite sprite)
            {
                background.sprite = sprite;
            });

        assetService.OnComplete += OnAssetLoadingComplete;
    }

    private void OnAssetLoadingComplete()
    {
        GlobalState.Instance.Services.Get<AssetService>().OnComplete -= OnAssetLoadingComplete;
        GlobalState.EventService.Dispatch(new LevelLoadedEvent());
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

        GlobalState.EventService.Dispatch(new LevelCompleteEvent(true));
        UpdateUser();

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

    private void UpdateUser()
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

        if (levelState.levelNumber == user.maxLevel)
        {
            user.maxLevel++;

            var sceneService = GlobalState.Instance.Services.Get<SceneService>();
            sceneService.PostTransitionCallbacks.Add(DispatchMovePlayerAvatar);
        }
    }

    private void DispatchReturnToMap()
    {
        GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
    }

    private void DispatchMovePlayerAvatar()
    {
        GlobalState.EventService.Dispatch(new MovePlayerAvatarEvent());
    }

    private void AddModifier(LevelModifierType type, string data)
    {
        var modifier = modifierFactory.Create(type);

        if (modifier != null)
        {
            modifiers.Add(modifier);
            modifier.SetData(data);
        }
    }
}
