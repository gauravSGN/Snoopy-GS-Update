using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelState LevelState { get { return levelState; } }

    public TextAsset levelData;
    public LevelLoader loader;
    public BubbleFactory bubbleFactory;
    private LevelState levelState = new LevelState();

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

        EventDispatcher.Instance.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        EventDispatcher.Instance.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
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
