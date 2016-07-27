using System;
using UnityEngine;
using System.Collections;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Coins : StateHandler
    {
        private const string QUANTITY = "quantity";

        public long quantity
        {
            get { return GetValue<long>(QUANTITY, GlobalState.Instance.Config.purchasables.newUserCoins); }
            set { SetValue<long>(QUANTITY, Math.Max(value, 0)); }
        }

        public Coins(Data topLevelState) : this(topLevelState, null) {}

        public Coins(Data topLevelState, Action<Observable> initialListener) : base(topLevelState, initialListener)
        {
        }
    }
}