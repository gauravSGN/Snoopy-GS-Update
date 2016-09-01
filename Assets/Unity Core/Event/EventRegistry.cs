using System;
using System.Collections.Generic;

namespace Event
{
    sealed public class EventRegistry
    {
        private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();

        public void Clear()
        {
            handlers.Clear();
        }

        public void AddEventHandler<T>(Action<T> handler) where T : GameEvent
        {
            var eventType = typeof(T);

            if (!handlers.ContainsKey(eventType))
            {
                handlers.Add(eventType, new List<object>());
            }

            var handlerList = handlers[eventType];

            if (!handlerList.Contains(handler))
            {
                handlerList.Add(handler);
            }
        }

        public List<object> RemoveEventHandler<T>(Action<T> handler) where T : GameEvent
        {
            List<object> handlerList = null;

            if (handlers.TryGetValue(typeof(T), out handlerList))
            {
                var index = handlerList.IndexOf(handler);
                if (index >= 0)
                {
                    handlerList[index] = null;
                }
                else
                {
                    handlerList = null;
                }
            }

            return handlerList;
        }

        public void Dispatch<T>(T gameEvent) where T : GameEvent
        {
            List<object> handlerList = null;

            if (handlers.TryGetValue(typeof(T), out handlerList))
            {
                for (int handlerIndex = 0, maxIndex = handlerList.Count; handlerIndex < maxIndex; handlerIndex++)
                {
                    if (handlerList[handlerIndex] != null)
                    {
                        (handlerList[handlerIndex] as Action<T>).Invoke(gameEvent);
                    }
                }
            }
        }
    }
}
