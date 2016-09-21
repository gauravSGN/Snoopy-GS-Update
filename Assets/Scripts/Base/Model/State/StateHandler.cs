using System;
using Data = System.Collections.Generic.IDictionary<string, object>;
using System.Collections.Generic;

namespace State
{
    public class StateHandler : Observable
    {
        protected Data state;
        protected bool initialized;

        virtual public string Key { get; set; }

        virtual public void Initialize(Data topLevelState)
        {
            if (!initialized)
            {
                if (topLevelState != null)
                {
                    state = topLevelState;

                    if (Key != null)
                    {
                        if (!topLevelState.ContainsKey(Key))
                        {
                            topLevelState[Key] = new Dictionary<string, object>();
                        }

                        state = (Data)topLevelState[Key];
                    }
                }

                initialized = true;
            }
        }

        protected T GetValue<T>(string key, T defaultValue)
        {
            return state.ContainsKey(key) ? (T)state[key] : defaultValue;
        }

        virtual protected void SetValue<T>(string key, object value)
        {
            state[key] = (T)value;
            NotifyListeners();
        }

        protected void NotifyListenersCallback(Observable target)
        {
            NotifyListeners();
        }

        protected T BuildStateHandler<T>() where T : StateHandler
        {
            return BuildStateHandler<T>(null);
        }

        virtual protected T BuildStateHandler<T>(Data topLevelState) where T : StateHandler
        {
            return BuildStateHandlerWithCallback<T>(topLevelState, NotifyListenersCallback);
        }

        protected T BuildStateHandlerWithCallback<T>(Data topLevelState,
                                                     Action<Observable> initialCallback) where T : StateHandler
        {
            var handlerType = typeof(T);
            T stateHandler = (T)Activator.CreateInstance(handlerType);

            stateHandler.Initialize(topLevelState);

            if (initialCallback != null)
            {
                stateHandler.AddListener(initialCallback);
            }

            return stateHandler;
        }
    }
}