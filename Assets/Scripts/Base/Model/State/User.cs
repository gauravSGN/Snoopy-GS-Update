using gs;
using System;
using Service;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class User : StateHandler, UserStateService
    {
        private const string HAS_PAID = "hasPaid";
        private const string MAX_LEVEL = "maxLevel";
        private const string CURRENT_LEVEL = "currentLevel";

        public long currentLevel
        {
            get { return GetValue<long>(CURRENT_LEVEL, 1); }
            set { SetValue<long>(CURRENT_LEVEL, value); }
        }

        public long maxLevel
        {
            get { return GetValue<long>(MAX_LEVEL, 1); }
            set { SetValue<long>(MAX_LEVEL, Math.Min(value, GlobalState.Instance.Config.level.maxLevel)); }
        }

        public long hasPaid
        {
            get { return GetValue<long>(HAS_PAID, 0); }
            set
            {
                if (hasPaid == 0)
                {
                    SetValue<long>(HAS_PAID, value);
                }
            }
        }

        public Levels levels { get; private set; }
        public Settings settings { get; private set; }
        public Purchasables purchasables { get; private set; }

        public User()
        {
            Initialize(GS.Api.State);
        }

        override public void Initialize(Data topLevelState)
        {
            if (!initialized)
            {
                base.Initialize(topLevelState);

                levels = BuildStateHandlerWithCallback<Levels>(GS.Api.State, UpdateAndNotifyListenersCallback);
                settings = BuildStateHandler<Settings>();
                purchasables = BuildStateHandler<Purchasables>();
            }
        }

        override protected void SetValue<T>(string key, object value)
        {
            state[key] = (T)value;
            Update();
            NotifyListeners();
        }

        private void UpdateAndNotifyListenersCallback(Observable target)
        {
            Update();
            NotifyListeners();
        }

        private void Update()
        {
            GS.Api.Update();
        }
    }
}