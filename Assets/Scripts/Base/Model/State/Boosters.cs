using System;

namespace State
{
    public class Boosters : StateHandler
    {
        private const string RAINBOWS = "rainbows";

        override public string Key { get { return "boosters"; } }

        public long rainbows
        {
            get { return GetValue<long>(RAINBOWS, 0); }
            set { SetValue<long>(RAINBOWS, Math.Max(value, 0)); }
        }
    }
}