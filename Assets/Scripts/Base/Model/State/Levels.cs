using System;
using System.Collections.Generic;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Levels : StateHandler
    {
        private Dictionary<int, LevelData> levelDataMap = new Dictionary<int, LevelData>();

        public LevelData this[int level]
        {
            get
            {
                if (!levelDataMap.ContainsKey(level))
                {
                    var levelAsString = level.ToString();

                    InitializeChildObjectIfNecessary(levelAsString);

                    levelDataMap[level] = new LevelData((Data)state[levelAsString], NotifyListenersCallback);
                }

                return levelDataMap[level];
            }
        }

        public Levels(Data state, Action<Observable> initialListener = null) : base(state, initialListener)
        {
        }
    }
}