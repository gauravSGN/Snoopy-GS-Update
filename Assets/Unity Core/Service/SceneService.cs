using System;
using System.Collections.Generic;

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
        List<Action> PostTransitionCallbacks { get; set; }
        float Progress { get; }

        void Reset();
        void TransitionToScene(string sceneName, bool startImmediately = true);
        void AllowTransition();
    }
}
