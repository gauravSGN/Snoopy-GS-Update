using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    public readonly LevelState levelState = new LevelState();
    public BubbleFactory bubbleFactory;

    [SerializeField]
    private TextAsset levelAsset;

    [SerializeField]
    private LevelLoader loader;

    private string levelData;

    protected void Start()
    {
        if ((GlobalState.Instance != null) && !string.IsNullOrEmpty(GlobalState.Instance.nextLevelData))
        {
            levelData = GlobalState.Instance.nextLevelData;
            GlobalState.Instance.nextLevelData = null;
        }
        else if (levelAsset != null)
        {
            levelData = levelAsset.text;
        }

        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForEndOfFrame();

        levelState.typeTotals = loader.LoadLevel(levelData);

        levelState.score = 0;
        levelState.remainingBubbles = loader.LevelData.ShotCount;
        levelState.NotifyListeners();

        GlobalState.Instance.EventDispatcher.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        GlobalState.Instance.EventDispatcher.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        GlobalState.Instance.EventDispatcher.AddEventHandler<GoalCompleteEvent>(OnGoalComplete);
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

        GlobalState.Instance.EventDispatcher.Dispatch(new LevelCompleteEvent(true));
    }
}
