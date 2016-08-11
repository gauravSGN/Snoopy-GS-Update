using Event;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Service
{
    public class SceneHandler : SceneService
    {
        public int LevelNumber { get; set; }
        public string NextLevelData { get; set; }
        public string ReturnScene { get; set; }
        public List<Action> PostTransitionCallbacks { get; set; }

        public SceneHandler()
        {
            ResetCallbacks();
            GlobalState.EventService.AddEventHandler<TransitionToReturnSceneEvent>(OnTransitionToReturnScene, HandlerDictType.Persistent);
        }

        public void Reset()
        {
            LevelNumber = -1;
            ReturnScene = "";
            NextLevelData = "";
            ResetCallbacks();
        }

        public void TransitionToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);

            foreach (var callback in PostTransitionCallbacks)
            {
                callback.Invoke();
            }

            ResetCallbacks();
        }

        private void OnTransitionToReturnScene(TransitionToReturnSceneEvent gameEvent)
        {
            TransitionToScene(ReturnScene);
        }

        private void ResetCallbacks()
        {
            PostTransitionCallbacks = new List<Action>();
        }
    }
}
