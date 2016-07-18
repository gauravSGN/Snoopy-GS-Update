using Service;
using UnityEngine;

public class TopUIManager : MonoBehaviour, TopUIService
{
    [SerializeField]
    private GameObject topUIObject;

    [SerializeField]
    private GameObject loadingPrefab;

    private GameObject loadingScreenInstance;
    private PopupManager popupManager;

    public void Start()
    {
        GlobalState.Instance.Services.SetInstance<TopUIService>(this);
        popupManager = topUIObject.GetComponent<PopupManager>();
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

    public void ShowOneButtonPopup(OneButtonPopupConfig config)
    {
        popupManager.ShowOneButtonPopupImmediate(config);
    }
}
