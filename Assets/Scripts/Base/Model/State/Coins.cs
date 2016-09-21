using System;

namespace State
{
    public class Coins : StateHandler
    {
        private const string QUANTITY = "quantity";

        override public string Key { get { return "coins"; } }

        public long quantity
        {
            get { return GetValue<long>(QUANTITY, GlobalState.Instance.Config.purchasables.newUserCoins); }
            set { SetValue<long>(QUANTITY, Math.Max(value, 0)); }
        }
    }
}