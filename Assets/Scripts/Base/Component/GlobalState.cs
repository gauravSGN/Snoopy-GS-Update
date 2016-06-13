using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get { return instance; } }
    private static GlobalState instance;

    public TextAsset nextLevelData;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}