using UnityEngine;

public class StartupTest : MonoBehaviour
{
    public string name;

    void Awake()
    {
        Debug.Log(name+" Awake");
    }

    void OnEnable()
    {
        Debug.Log(name+" Enable");
    }

    void Start()
    {
        Debug.Log(name+" Start");
    }
}
