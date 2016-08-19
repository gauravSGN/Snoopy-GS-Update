using Service;
using Registry;
using UnityEngine;

public class BlockadeComponent : MonoBehaviour, Blockade
{
    [SerializeField]
    private BlockadeType type;

    public BlockadeType BlockadeType { get { return type; } }

    public void Start()
    {
        GlobalState.Instance.Services.Get<BlockadeService>().Add(this);
    }

    public void OnDestroy()
    {
        GlobalState.Instance.Services.Get<BlockadeService>().Remove(this);
    }
}
