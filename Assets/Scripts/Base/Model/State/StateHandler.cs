using System;
using Data = System.Collections.Generic.IDictionary<string, object>;
using System.Collections.Generic;

namespace State
{
    public class StateHandler : Observable
    {
        protected Data state;

        public StateHandler(Data topLevelState, Action<Observable> initialListener = null)
        {
            state = topLevelState;

            if (initialListener != null)
            {
                AddListener(initialListener);
            }
        }

        protected T GetValue<T>(string key, T defaultValue)
        {
            return state.ContainsKey(key) ? (T)state[key] : defaultValue;
        }

        protected void SetValue<T>(string key, object value)
        {
            state[key] = (T)value;
            NotifyListeners();
        }

        protected void InitializeChildObjectIfNecessary(string key, object value = null)
        {
            if (!state.ContainsKey(key))
            {
                state[key] = value ?? new Dictionary<string, object>();
            }
        }

        protected void NotifyListenersCallback(Observable target)
        {
            NotifyListeners();
        }
    }
}