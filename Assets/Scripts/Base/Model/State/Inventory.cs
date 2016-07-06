using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Inventory : StateHandler
    {
        public uint rainbowPowerups
        {
            get { return GetValue<uint>("rainbowPowerups", 0); }
            set { SetValue<uint>("rainbowPowerups", value); }
        }

        public Inventory(Data state, Action<Observable> initialListener = null) : base(state, initialListener)
        {
        }
    }
}