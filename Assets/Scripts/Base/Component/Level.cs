using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    public LevelState LevelState { get { return levelState; } }

    public TextAsset levelData;
    public LevelLoader loader;
    public BubbleFactory bubbleFactory;

    private LevelState levelState = new LevelState();

    protected void Start()
    {
        loader.LoadLevel(levelData);

        levelState.score = 0;
        levelState.remainingBubbles = loader.LevelData.remainingBubble;
        levelState.NotifyListeners();

        EventDispatcher.Instance.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        EventDispatcher.Instance.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
    }

    private void OnBubbleFired(BubbleFiredEvent gameEvent)
    {
        levelState.remainingBubbles--;
        levelState.NotifyListeners();
    }

    private void OnBubbleDestroyed(BubbleDestroyedEvent gameEvent)
    {
        LevelState.score += gameEvent.score;
        LevelState.NotifyListeners();
    }
}
