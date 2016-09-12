using Util;
using System;
using System.Linq;
using Event.Invocation;
using System.Collections.Generic;

namespace Event
{
    sealed public class EventRegistry
    {
        private readonly Dictionary<Type, List<Invoker>> handlers = new Dictionary<Type, List<Invoker>>();
        private readonly ObjectPool<Invoker> invokerPool = new ObjectPool<Invoker>();

        public void Clear()
        {
            handlers.Clear();
            invokerPool.Clear();
        }

        public void AddEventHandler(Type eventType, Action handler)
        {
            var invoker = invokerPool.Get<ParameterlessInvoker>();

            invoker.Handler = handler;

            RegisterInvoker(eventType, invoker);
        }

        public void AddEventHandler<T>(Action handler) where T : GameEvent
        {
            AddEventHandler(typeof(T), handler);
        }

        public void AddEventHandler<T>(Action<T> handler) where T : GameEvent
        {
            var invoker = invokerPool.Get<SpecificInvoker<T>>();

            invoker.Handler = handler;

            RegisterInvoker(typeof(T), invoker);
        }

        public List<Invoker> RemoveEventHandler(Type eventType, object handler)
        {
            List<Invoker> handlerList = null;

            if (handlers.TryGetValue(eventType, out handlerList))
            {
                var index = handlerList.FindIndex(i => (i != null) && i.Target.Equals(handler));
                if (index >= 0)
                {
                    invokerPool.Release(handlerList[index]);
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
            List<Invoker> handlerList = null;

            if (handlers.TryGetValue(typeof(T), out handlerList))
            {
                for (int handlerIndex = 0, maxIndex = handlerList.Count; handlerIndex < maxIndex; handlerIndex++)
                {
                    if (handlerList[handlerIndex] != null)
                    {
                        handlerList[handlerIndex].Invoke(gameEvent);
                    }
                }
            }
        }

        private void RegisterInvoker(Type eventType, Invoker invoker)
        {
            if (!handlers.ContainsKey(eventType))
            {
                handlers.Add(eventType, new List<Invoker>());
            }

            var handlerList = handlers[eventType];

            if (!handlerList.Any(i => (i != null) && i.Target.Equals(invoker.Target)))
            {
                handlerList.Add(invoker);
            }
            else
            {
                invokerPool.Release(invoker);
            }
        }
    }
}
