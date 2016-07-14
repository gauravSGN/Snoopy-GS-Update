using System;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Purchasables : PersistableStateHandler
    {
        private const string HEARTS = "hearts";

        public Hearts hearts { get; private set; }

        public Purchasables(Data topLevelState, Action<Observable> initialListener = null) : base(topLevelState, initialListener)
        {
            InitializeStateKeys();
            hearts = new Hearts((Data)state[HEARTS], SaveAndNotifyListenersCallback);
        }

        override protected string[] GetStateKeys()
        {
            return new string[] {HEARTS};
        }
    }
}