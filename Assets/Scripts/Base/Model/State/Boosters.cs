using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Boosters : StateHandler
    {
        private const string RAINBOWS = "rainbows";

        public long rainbows
        {
            get { return GetValue<long>(RAINBOWS, 0); }
            set { SetValue<long>(RAINBOWS, Math.Max(value, 0)); }
        }

        public Boosters(Data topLevelState) : this(topLevelState, null) {}

        public Boosters(Data topLevelState, Action<Observable> initialListener) : base(topLevelState, initialListener)
        {
        }
    }
}