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

        private readonly List<IEnumerator> loadingRoutines = new List<IEnumerator>();
        private UnityEngine.AsyncOperation asyncOp;

        public int LevelNumber { get; set; }
        public string NextLevelData { get; set; }
        public string ReturnScene { get; set; }
        public float Progress { get; private set; }

        public SceneHandler()
        {
            GlobalState.EventService.Persistent.AddEventHandler<TransitionToReturnSceneEvent>(OnTransitionToReturnScene);
        }

        public void Reset()
        {
            LevelNumber = -1;
            ReturnScene = "";
            NextLevelData = "";
        }

        public void TransitionToScene(string sceneName, bool startImmediately = true)
        {
            if (asyncOp == null)
            {
                GlobalState.Instance.RunCoroutine(LoadScene(sceneName, startImmediately));
            }
            else
            {
                throw new Exception("Already loading a new scene.");
            }
        }

        public void AllowTransition()
        {
            if ((asyncOp != null) && !asyncOp.allowSceneActivation)
            {
                asyncOp.allowSceneActivation = true;
            }
        }

        public void RunAtLoad(IEnumerator coroutine)
        {
            loadingRoutines.Add(coroutine);
        }

        private void OnTransitionToReturnScene()
        {
            TransitionToScene(ReturnScene);
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
            Progress = 1.0f;

            yield return null;

            var assetService = GlobalState.AssetService;

            while (assetService.IsLoading || (loadingRoutines.Count > 0))
            {
                var index = 0;

                while (index < loadingRoutines.Count)
                {
                    if (!loadingRoutines[index].MoveNext())
                    {
                        loadingRoutines.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }

                yield return null;
            }

            if (OnFinishedLoading != null)
            {
                OnFinishedLoading();
            }
        }
    }
}
