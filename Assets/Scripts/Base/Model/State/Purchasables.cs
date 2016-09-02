using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Purchasables : PersistableStateHandler
    {
        public Coins coins { get; private set; }
        public Hearts hearts { get; private set; }
        public Boosters boosters { get; private set; }

        override public void Initialize(Data topLevelState)
        {
            if (!initialized)
            {
                base.Initialize(topLevelState);

                coins = BuildStateHandler<Coins>(state);
                hearts = BuildStateHandler<Hearts>(state);
                boosters = BuildStateHandler<Boosters>(state);
            }
        }

        override protected T BuildStateHandler<T>(Data topLevelState)
        {
            return BuildStateHandlerWithCallback<T>(topLevelState, SaveAndNotifyListenersCallback);
        }
    }
}