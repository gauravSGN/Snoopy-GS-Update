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

        levelState.typeTotals = loader.LoadLevel(levelData);

        levelState.score = 0;
        levelState.remainingBubbles = loader.LevelData.remainingBubble;
        levelState.NotifyListeners();

        GlobalState.Instance.EventDispatcher.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        GlobalState.Instance.EventDispatcher.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
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
}
