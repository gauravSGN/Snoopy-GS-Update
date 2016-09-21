using System;
using System.Collections;

namespace Service
{
    public interface SceneService : SharedService
    {
        event Action OnStartLoading;
        event Action OnSceneReady;
        event Action OnFinishedLoading;

        int LevelNumber { get; set; }
        string NextLevelData { get; set; }
        string ReturnScene { get; set; }
        float Progress { get; }

        void Reset();
        void TransitionToScene(string sceneName, bool startImmediately = true);
        void AllowTransition();
        void RunAtLoad(IEnumerator coroutine);
    }
}
