using UnityEngine;

// The GlobalState prefab needs to be in every scene that uses it for the
// scene editor to work without coming from a different scenes.
public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; private set; }
    public EventDispatcher EventDispatcher { get; private set; }

    public TextAsset nextLevelData;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            EventDispatcher = new EventDispatcher();
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if ((Instance != null) && (this == Instance))
        {
            EventDispatcher.Reset();
        }
    }
}