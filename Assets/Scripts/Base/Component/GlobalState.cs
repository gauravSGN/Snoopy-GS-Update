using UnityEngine;

// The GlobalState prefab needs to be in every scene that uses it for the
// scene editor to work without coming from a different scenes.
public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get { return instance; } }
    private static GlobalState instance;

    public EventDispatcher EventDispatcher { get { return eventDispatcher; } }
    private EventDispatcher eventDispatcher;

    public TextAsset nextLevelData;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            eventDispatcher = new EventDispatcher();
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if ((instance != null) && (this == instance))
        {
            eventDispatcher.Reset();
        }
    }
}