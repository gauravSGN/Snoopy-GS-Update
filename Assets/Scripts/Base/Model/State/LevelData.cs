using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class LevelData : StateHandler
    {
        public uint score
        {
            get { return GetValue<uint>("score", 0); }
            set { SetValue<uint>("score", value); }
        }

        public uint stars
        {
            get { return GetValue<uint>("stars", 0); }
            set { SetValue<uint>("stars", value); }
        }

        public uint updatedAt
        {
            get { return GetValue<uint>("updatedAt", 0); }
            set { SetValue<uint>("updatedAt", value); }
        }

        public LevelData(Data state, Action<Observable> initialListener = null) : base(state, initialListener)
        {
        }
    }
}