using System;
using System.Collections.Generic;

namespace Service
{
    public interface SceneService : SharedService
    {
        int LevelNumber { get; set; }
        string NextLevelData { get; set; }
        string ReturnScene { get; set; }
        List<Action> PostTransitionCallbacks { get; set; }

        void Reset();
        void TransitionToScene(string sceneName);
    }
}
