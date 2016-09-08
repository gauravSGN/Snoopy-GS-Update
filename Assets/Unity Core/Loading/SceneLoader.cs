using UnityEngine;
using UnityEngine.Events;

namespace Loading
{
    sealed public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        [SerializeField]
        private bool loadOnAwake;

        [SerializeField]
        private bool startImmediately;

        [SerializeField]
        private UnityEvent onReady;

        [SerializeField]
        private UnityEvent onComplete;

        public void Start()
        {
            if (loadOnAwake)
            {
                LoadScene();
            }
        }

        public void LoadScene()
        {
            GlobalState.SceneService.OnFinishedLoading += OnFinishLoadingScene;

            if (!startImmediately)
            {
                GlobalState.SceneService.OnSceneReady += OnSceneReady;
            }

            GlobalState.SceneService.TransitionToScene(sceneName, startImmediately);
        }

        private void OnFinishLoadingScene()
        {
            GlobalState.SceneService.OnFinishedLoading -= OnFinishLoadingScene;

            onComplete.Invoke();
        }

        private void OnSceneReady()
        {
            GlobalState.SceneService.OnSceneReady -= OnSceneReady;

            onReady.Invoke();
        }
    }
}
