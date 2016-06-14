using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    public readonly LevelState levelState = new LevelState();
    public BubbleFactory bubbleFactory;

    [SerializeField]
    private TextAsset levelData;

    [SerializeField]
    private LevelLoader loader;

    protected void Start()
    {
        if (GlobalState.Instance && GlobalState.Instance.nextLevelData)
        {
            levelData = GlobalState.Instance.nextLevelData;
            GlobalState.Instance.nextLevelData = null;
        }

        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        levelState.typeTotals = loader.LoadLevel(levelData);

        levelState.score = 0;
        levelState.remainingBubbles = loader.LevelData.remainingBubble;
        levelState.NotifyListeners();

        EventDispatcher.Instance.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        EventDispatcher.Instance.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        EventDispatcher.Instance.AddEventHandler<GoalCompleteEvent>(OnGoalComplete);
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

        EventDispatcher.Instance.Dispatch(new LevelCompleteEvent(true));
    }
}
