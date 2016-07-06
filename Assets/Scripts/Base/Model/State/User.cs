using gs;
using Service;
using System.Collections.Generic;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class User : StateHandler, UserStateService
    {
        public uint currentLevel
        {
            get { return GetValue<uint>("currentLevel", 1); }
            set { SetValue<uint>("currentLevel", value); }
        }

        public uint maxLevel
        {
            get { return GetValue<uint>("maxLevel", 1); }
            set { SetValue<uint>("maxLevel", value); }
        }

        public uint hasPaid
        {
            get { return GetValue<uint>("hasPaid", 0); }
            set { SetValue<uint>("hasPaid", value); }
        }

        public Inventory inventory { get; private set; }
        public Levels levels { get; private set; }

        public User() : base(GS.Api.State)
        {
            string[] dataStateKeys = {"inventory", "ftue", "levels"};

            foreach (var dataStateKey in dataStateKeys)
            {
                InitializeChildObjectIfNecessary(dataStateKey);
            }

            inventory = new Inventory((Data)GS.Api.State["inventory"], NotifyListenersCallback);
            levels = new Levels((Data)GS.Api.State["levels"], NotifyListenersCallback);

            // ftue
        }
    }
}