using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Purchasables : PersistableStateHandler
    {
        private const string COINS = "coins";
        private const string HEARTS = "hearts";

        public Coins coins { get; private set; }
        public Hearts hearts { get; private set; }

        public Purchasables(Data topLevelState) : this(topLevelState, null) {}

        public Purchasables(Data topLevelState, Action<Observable> initialListener)
            : base(topLevelState, initialListener)
        {
            InitializeStateKeys();

            coins = new Coins((Data)state[COINS], SaveAndNotifyListenersCallback);
            hearts = new Hearts((Data)state[HEARTS], SaveAndNotifyListenersCallback);
        }

        override protected string[] GetStateKeys()
        {
            return new[] { COINS, HEARTS };
        }
    }
}