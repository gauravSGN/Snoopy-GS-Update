using System;
using Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Loading
{
    public class SceneHandler : SceneService
    {
        // For details, see: https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
        private const float MAGIC_UNITY_LOADING_VALUE = 0.9f;

        public event Action OnStartLoading;
        public event Action OnSceneReady;
        public event Action OnFinishedLoading;

        public int LevelNumber { get; set; }
        public string NextLevelData { get; set; }
        public string ReturnScene { get; set; }
        public List<Action> PostTransitionCallbacks { get; set; }
        public float Progress { get; private set; }

        private UnityEngine.AsyncOperation asyncOp;

        public SceneHandler()
        {
            ResetCallbacks();
            GlobalState.EventService.Persistent.AddEventHandler<TransitionToReturnSceneEvent>(OnTransitionToReturnScene);
        }

        public void Reset()
        {
            LevelNumber = -1;
            ReturnScene = "";
            NextLevelData = "";
            ResetCallbacks();
        }

        public void TransitionToScene(string sceneName, bool startImmediately = true)
        {
            GlobalState.Instance.RunCoroutine(LoadScene(sceneName, startImmediately));
        }

        public void AllowTransition()
        {
            if ((asyncOp != null) && !asyncOp.allowSceneActivation)
            {
                asyncOp.allowSceneActivation = true;
            }
        }

        // What is better than this?
        // Maybe the SceneManager.sceneLoaded callbacks in 5.4 will work?
        // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
        private IEnumerator InvokePostTransitionCallbacks()
        {
            yield return 1;

            foreach (var callback in PostTransitionCallbacks)
            {
                callback.Invoke();
            }

            ResetCallbacks();
        }

        private void OnTransitionToReturnScene()
        {
            TransitionToScene(ReturnScene);
        }

        private void ResetCallbacks()
        {
            PostTransitionCallbacks = new List<Action>();
        }

        private IEnumerator LoadScene(string sceneName, bool startImmediately)
        {
            if (OnStartLoading != null)
            {
                OnStartLoading();
            }

            yield return null;

            asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = startImmediately;

            while (!asyncOp.isDone)
            {
                yield return null;

                if (!startImmediately &&
                    (Progress < MAGIC_UNITY_LOADING_VALUE) &&
                    (asyncOp.progress >= MAGIC_UNITY_LOADING_VALUE))
                {
                    if (OnSceneReady != null)
                    {
                        OnSceneReady();
                    }
                }

                Progress = asyncOp.progress;
            }

            asyncOp = null;

//            GlobalState.Instance.RunCoroutine(InvokePostTransitionCallbacks());

            var assetService = GlobalState.AssetService;
            while (assetService.IsLoading)
            {
                yield return null;
            }

            yield return null;

            if (OnFinishedLoading != null)
            {
                OnFinishedLoading();
            }
        }
    }
}
