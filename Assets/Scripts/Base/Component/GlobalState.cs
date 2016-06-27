using Util;
using Config;
using UnityEngine;

// The GlobalState prefab needs to be in every scene that uses it for the
// scene editor to work without coming from a different scene.
[RequireComponent(typeof(UpdateDispatcher))]
public class GlobalState : SingletonBehaviour<GlobalState>
{
    public EventDispatcher EventDispatcher { get; private set; }
    public UpdateDispatcher UpdateDispatcher { get; private set; }
    public GameConfig Config { get { return config; } }

    public string nextLevelData;
    public string returnScene;

    [SerializeField]
    private GameConfig config;

    [SerializeField]
    private TextAsset gsDescriptorJSON;

    private GSDescriptor gsDescriptor;

    override protected void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            EventDispatcher = new EventDispatcher();
            UpdateDispatcher = GetComponent<UpdateDispatcher>();
        }
    }

    // Initialize things in Start if they will dispatch events
    protected void Start()
    {
        if (Instance == this)
        {
            gsDescriptor = GSDescriptorFactory.CreateByPlatform(Application.platform, gsDescriptorJSON);
            gsDescriptor.Initialize();
        }
    }

    protected void OnLevelWasLoaded(int level)
    {
        if (this == Instance)
        {
            EventDispatcher.Reset();
            UpdateDispatcher.Reset();
        }
    }
}
