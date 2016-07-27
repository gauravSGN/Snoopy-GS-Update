using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class LevelData : StateHandler
    {
        public long score
        {
            get { return GetValue<long>("score", 0); }
            set { SetValue<long>("score", value); }
        }

        public long stars
        {
            get { return GetValue<long>("stars", 0); }
            set { SetValue<long>("stars", value); }
        }

        public long updatedAt
        {
            get { return GetValue<long>("updatedAt", 0); }
            set { SetValue<long>("updatedAt", value); }
        }

        public LevelData(Data topLevelState) : this(topLevelState, null) {}

        public LevelData(Data topLevelState, Action<Observable> initialListener) : base(topLevelState, initialListener)
        {
        }
    }
}