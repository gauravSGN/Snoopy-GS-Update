using Util;
using Config;
using UnityEngine;
using Service;
using System.Collections;

// The GlobalState prefab needs to be in every scene that uses it for the
// scene editor to work without coming from a different scene.
public class GlobalState : SingletonBehaviour<GlobalState>
{
    public static EventService EventService { get { return Instance.Services.Get<EventService>(); } }

    public GameConfig Config { get { return config; } }
    public ServiceRepository Services { get { return services; } }

    [SerializeField]
    private GameConfig config;

    [SerializeField]
    private TextAsset gsDescriptorJSON;

    [SerializeField]
    private TextAsset servicesJSON;

    private readonly ServiceRepository services = new ServiceRepository();

    override protected void Awake()
    {
        Services.RegisterFromJson(servicesJSON.text);

        base.Awake();
    }

    public void RunCoroutine(IEnumerator enumerator)
    {
        if (Instance == this)
        {
            StartCoroutine(enumerator);
        }
    }

    // Initialize things in Start if they will dispatch events
    protected void Start()
    {
        if (Instance == this)
        {
            GSDescriptorFactory.CreateByPlatform(Application.platform, gsDescriptorJSON).Initialize();
            Application.targetFrameRate = 60;
        }
    }

    protected void OnLevelWasLoaded(int level)
    {
        if (this == Instance)
        {
            Services.Get<EventService>().Reset();
            Services.Get<UpdateService>().Reset();
        }
    }
}
