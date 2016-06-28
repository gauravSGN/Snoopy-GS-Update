using UnityEngine;

public class TopUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingPrefab;

    private GameObject loadingScreenInstance;

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
