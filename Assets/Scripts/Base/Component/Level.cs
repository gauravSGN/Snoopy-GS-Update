using Util;
using Model;
using System;
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

    public bool AllGoalsCompleted { get; private set; }

    [SerializeField]
    private string levelAssetPath;

    [SerializeField]
    private LevelLoader loader;

    [SerializeField]
    private SpriteRenderer background;

    private readonly List<LevelModifier> modifiers = new List<LevelModifier>();
    private readonly ModifierFactory modifierFactory = new ModifierFactory();
    private string levelData;

    public void UpdateUser()
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
            GlobalState.SceneService.PostTransitionCallbacks.Add(DispatchMovePlayerAvatar);
        }
    }

    protected void Start()
    {
        var sceneData = GlobalState.SceneService;

        if (!string.IsNullOrEmpty(sceneData.NextLevelData))
        {
            levelData = sceneData.NextLevelData;
            sceneData.NextLevelData = null;
        }
        else if (levelAssetPath != null)
        {
            levelData = GlobalState.AssetService.LoadAsset<TextAsset>(levelAssetPath).text;
        }

        levelState.levelNumber = sceneData.LevelNumber;
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        bubbleFactory.ResetModifiers();
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

        GlobalState.EventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        GlobalState.EventService.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        GlobalState.EventService.AddEventHandler<GoalCompleteEvent>(OnGoalComplete);
        GlobalState.EventService.AddEventHandler<AddLevelModifierEvent>(e => AddModifier(e.type, e.data));

        GlobalState.AssetService.OnComplete += OnAssetLoadingComplete;

        GlobalState.AssetService.LoadAssetAsync<Sprite>(loader.LevelData.Background, delegate(Sprite sprite)
            {
                background.sprite = sprite;
            });
    }

    private void OnAssetLoadingComplete()
    {
        GlobalState.AssetService.OnComplete -= OnAssetLoadingComplete;
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

        AllGoalsCompleted = true;
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
