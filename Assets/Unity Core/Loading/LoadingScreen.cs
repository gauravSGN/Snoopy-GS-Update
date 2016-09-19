using Loading;
using Service;
using UnityEngine;
using UnityEngine.UI;

sealed public class LoadingScreen : MonoBehaviour, UpdateReceiver
{
    [SerializeField]
    private Image fillImage;

    public void Start()
    {
        gameObject.SetActive(false);

        var sceneService = GlobalState.SceneService;

        sceneService.OnStartLoading += OnStartLoadingScene;
        sceneService.OnFinishedLoading += OnFinishLoadingScene;
    }

    public void OnDestroy()
    {
        var sceneService = GlobalState.SceneService;

        sceneService.OnStartLoading -= OnStartLoadingScene;
        sceneService.OnFinishedLoading -= OnFinishLoadingScene;
    }

    public void OnUpdate()
    {
        var progress = GlobalState.SceneService.Progress;

        var assetService = GlobalState.AssetService;
        if (assetService.IsLoading)
        {
            progress = (progress + assetService.Progress) / 2.0f;
        }

        fillImage.fillAmount = progress;
    }

    private void OnStartLoadingScene()
    {
        gameObject.SetActive(true);

        GlobalState.UpdateService.Updates.Add(this);
    }

    private void OnFinishLoadingScene()
    {
        GlobalState.UpdateService.Updates.Remove(this);

        gameObject.SetActive(false);
    }
}
