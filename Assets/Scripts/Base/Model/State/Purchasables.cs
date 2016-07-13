using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

using UnityEngine;

namespace State
{
    public class Purchasables : PersistableStateHandler
    {
        public long hearts
        {
            get { return GetValue<long>("hearts", GlobalState.Instance.Config.purchasables.maxLives); }
            set { SetValue<long>("hearts", Math.Min(Math.Max(value, 0), GlobalState.Instance.Config.purchasables.maxLives)); }
        }

        public long lastHeartAwardTime
        {
            get { return GetValue<long>("lastHeartAwardTime", 0); }
            set { SetValue<long>("lastHeartAwardTime", value); }
        }

        public long rainbowPowerups
        {
            get { return GetValue<long>("rainbowPowerups", 0); }
            set { SetValue<long>("rainbowPowerups", value); }
        }

        public Purchasables(Data state, Action<Observable> initialListener = null) : base(state, initialListener)
        {
        }
    }
}