using Service;
using UnityEngine;

public class TopUIManager : MonoBehaviour, TopUIService
{
    [SerializeField]
    private GameObject topUIObject;

    [SerializeField]
    private GameObject loadingPrefab;

    private GameObject loadingScreenInstance;

    public void Start()
    {
        GlobalState.Instance.Services.SetInstance<TopUIService>(this);
    }

    public void ShowLoading(AsyncOperation op)
    {
        loadingScreenInstance = Instantiate(loadingPrefab);
        AsyncLoadingFill fill = loadingScreenInstance.GetComponent<AsyncLoadingFill>();
        fill.StartFill(op, DestroyWhenDone);
    }

    private void DestroyWhenDone(GameObject toDestroy)
    {
        Destroy(toDestroy);
    }
}
