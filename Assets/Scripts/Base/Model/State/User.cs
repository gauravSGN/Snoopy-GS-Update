using gs;
using Service;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class User : StateHandler, UserStateService
    {
        public long currentLevel
        {
            get { return GetValue<long>("currentLevel", 1); }
            set { SetValue<long>("currentLevel", value); }
        }

        public long maxLevel
        {
            get { return GetValue<long>("maxLevel", 1); }
            set { SetValue<long>("maxLevel", value); }
        }

        public long hasPaid
        {
            get { return GetValue<long>("hasPaid", 0); }
            set { SetValue<long>("hasPaid", value); }
        }

        public Inventory inventory { get; private set; }
        public Levels levels { get; private set; }

        public User() : base(GS.Api.State)
        {
            string[] dataStateKeys = {"inventory", "levels"};

            foreach (var dataStateKey in dataStateKeys)
            {
                InitializeChildObjectIfNecessary(dataStateKey);
            }

            inventory = new Inventory((Data)GS.Api.State["inventory"], NotifyListenersCallback);
            levels = new Levels((Data)GS.Api.State["levels"], NotifyListenersCallback);
        }
    }
}