using Service;
using Registry;
using UnityEngine;

public class RegisteredTransform : MonoBehaviour
{
    [SerializeField]
    private TransformUsage usage;

    public void Start()
    {
        GlobalState.Instance.Services.Get<TransformService>().Register(transform, usage);
    }

    public void OnDestroy()
    {
        GlobalState.Instance.Services.Get<TransformService>().Unregister(transform, usage);
    }
}
