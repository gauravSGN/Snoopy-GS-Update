using Util;
using System;
using Service;
using Event.Invocation;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Event
{
    public class EventDispatcher : EventService
    {
        private int dispatchesInProgress;
        private EventRegistry[] registries = new EventRegistry[2];

        private readonly ObjectPool<GameEvent> eventPool = new ObjectPool<GameEvent>();
        private readonly HashSet<List<Invoker>> handlerListsToClean = new HashSet<List<Invoker>>();

        public EventRegistry Transient { get; private set; }
        public EventRegistry Persistent { get; private set; }

        public EventDispatcher()
        {
            registries[0] = Transient = new EventRegistry();
            registries[1] = Persistent = new EventRegistry();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void AddEventHandler<T>(Action handler) where T : GameEvent
        {
            Transient.AddEventHandler<T>(handler);
        }

        public void AddEventHandler<T>(Action<T> handler) where T : GameEvent
        {
            Transient.AddEventHandler<T>(handler);
        }

        public void RemoveEventHandler<T>(Action handler) where T : GameEvent
        {
            RemoveEventHandlers(typeof(T), handler);
        }

        public void RemoveEventHandler<T>(Action<T> handler) where T : GameEvent
        {
            RemoveEventHandlers(typeof(T), handler);
        }

        public void Dispatch<T>(T gameEvent) where T : GameEvent
        {
            dispatchesInProgress++;

            foreach (var registry in registries)
            {
                registry.Dispatch<T>(gameEvent);
            }

            dispatchesInProgress--;
            CleanModifiedHandlerLists();
        }

        public void DispatchPooled<T>(T gameEvent) where T : GameEvent
        {
            Dispatch(gameEvent);
            AddPooledEvent(gameEvent);
        }

        public T GetPooledEvent<T>() where T : GameEvent
        {
            return eventPool.Get<T>();
        }

        public void AddPooledEvent<T>(T gameEvent) where T : GameEvent
        {
            eventPool.Release(gameEvent);
        }

        private void RemoveEventHandlers(Type eventType, object handler)
        {
            foreach (var registry in registries)
            {
                var listToClean = registry.RemoveEventHandler(eventType, handler);

                if (listToClean != null)
                {
                    handlerListsToClean.Add(listToClean);
                }
            }
        }

        private void CleanModifiedHandlerLists()
        {
            if ((dispatchesInProgress == 0) && (handlerListsToClean.Count > 0))
            {
                foreach (var handlerList in handlerListsToClean)
                {
                    handlerList.RemoveAll(h => (h == null));
                }

                handlerListsToClean.Clear();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Transient.Clear();
            eventPool.Clear();
        }
    }
}
