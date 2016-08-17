using UnityEngine;

public class InitializableBehaviour : MonoBehaviour, Initializable
{
    virtual public void Start()
    {
        RegisterForInitialization();
    }

    virtual public void PreInitialize()
    {
        // Default implementation does nothing so that users can choose which to override.
    }

    virtual public void Initialize()
    {
        // Default implementation does nothing so that users can choose which to override.
    }

    virtual public void PostInitialize()
    {
        // Default implementation does nothing so that users can choose which to override.
    }

    protected void RegisterForInitialization(params Initializable[] dependencies)
    {
        GlobalState.InitializerService.Register(this, dependencies);
    }

    private void Awake()
    {
        // Classes implementing Initializable should do their setup and registration from Start.
    }
}
