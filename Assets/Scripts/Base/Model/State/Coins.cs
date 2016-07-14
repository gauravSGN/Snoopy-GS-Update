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
            set { SetValue<long>(QUANTITY, value); }
        }

        public Coins(Data topLevelState, Action<Observable> initialListener = null) : base(topLevelState, initialListener)
        {
        }
    }
}