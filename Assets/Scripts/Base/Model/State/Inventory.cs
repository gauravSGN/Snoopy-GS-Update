using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Inventory : StateHandler
    {
        public long rainbowPowerups
        {
            get { return GetValue<long>("rainbowPowerups", 0); }
            set { SetValue<long>("rainbowPowerups", value); }
        }

        public Inventory(Data state, Action<Observable> initialListener = null) : base(state, initialListener)
        {
        }
    }
}