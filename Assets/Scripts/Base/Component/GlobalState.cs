using UnityEngine;
using Util;

// The GlobalState prefab needs to be in every scene that uses it for the
// scene editor to work without coming from a different scene.
[RequireComponent(typeof(UpdateDispatcher))]
public class GlobalState : SingletonBehaviour<GlobalState>
{
    public EventDispatcher EventDispatcher { get; private set; }
    public UpdateDispatcher UpdateDispatcher { get; private set; }

    public string nextLevelData;
    public string returnScene;

    override protected void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            EventDispatcher = new EventDispatcher();
            UpdateDispatcher = GetComponent<UpdateDispatcher>();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (this == Instance)
        {
            EventDispatcher.Reset();
            UpdateDispatcher.Reset();
        }
    }
}
