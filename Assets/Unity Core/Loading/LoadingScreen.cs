using Loading;
using Service;
using UnityEngine;
using UnityEngine.UI;

namespace Namespace
{
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
            fillImage.fillAmount = (GlobalState.AssetService.Progress + GlobalState.SceneService.Progress) / 2.0f;
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
}
