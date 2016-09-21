using System.Collections.Generic;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Levels : StateHandler
    {
        private readonly Dictionary<int, LevelData> levelDataMap = new Dictionary<int, LevelData>();

        override public string Key { get { return "levels"; } }

        public LevelData this[int level]
        {
            get
            {
                if (!levelDataMap.ContainsKey(level))
                {
                    levelDataMap[level] = BuildLevelDataStateHandler(level, state);
                }

                return levelDataMap[level];
            }
        }

        private LevelData BuildLevelDataStateHandler(int level, Data topLevelState)
        {
            LevelData levelDataStateHandler = new LevelData();

            levelDataStateHandler.Key = level.ToString();
            levelDataStateHandler.Initialize(topLevelState);
            levelDataStateHandler.AddListener(NotifyListenersCallback);

            return levelDataStateHandler;
        }
    }
}