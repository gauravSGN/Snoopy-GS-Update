using Util;

// The GlobalState prefab needs to be in every scene that uses it for the
// scene editor to work without coming from a different scenes.
public class GlobalState : SingletonBehaviour<GlobalState>
{
    public EventDispatcher EventDispatcher { get; private set; }

    public string nextLevelData;
    public string returnScene;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            EventDispatcher = new EventDispatcher();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (this == Instance)
        {
            EventDispatcher.Reset();
        }
    }
}
