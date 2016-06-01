using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    public LevelState LevelState { get { return levelState; } }

    public TextAsset levelData;
    public LevelLoader loader;

    private LevelState levelState = new LevelState();

    protected void Start()
    {
        loader.LoadLevel(levelData);

        levelState.remainingBubbles = loader.LevelData.remainingBubble;
        levelState.NotifyListeners();

        EventDispatcher.Instance.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
    }

    private void OnBubbleFired(BubbleFiredEvent gameEvent)
    {
        levelState.remainingBubbles--;
        levelState.NotifyListeners();
    }
}
