using UnityEngine;
using System;
using System.Collections.Generic;

public class EventDispatcher : MonoBehaviour
{
    public static EventDispatcher Instance { get { return instance; } }

    private static EventDispatcher instance;

    private Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();
    private Dictionary<Type, List<object>> pools = new Dictionary<Type, List<object>>();

    public void OnEnable()
    {
        instance = this;
    }

    public void OnDestroy()
    {
        handlers.Clear();
    }

    public void AddEventHandler<T>(Action<T> handler) where T : GameEvent
    {
        var eventType = typeof(T);
        List<object> handlerList;

        if (handlers.ContainsKey(eventType))
        {
            handlerList = handlers[eventType];
        }
        else
        {
            handlerList = new List<object>();
            handlers.Add(eventType, handlerList);
        }

        handlerList.Add(handler);
    }

    public void RemoveEventHandler<T>(Action<T> handler) where T : GameEvent
    {
        var eventType = typeof(T);

        if (handlers.ContainsKey(eventType))
        {
            var handlerList = handlers[eventType];

            if (handlerList.Contains(handler))
            {
                handlerList.Remove(handler);
            }
        }
    }

    public void Dispatch<T>(T gameEvent) where T : GameEvent
    {
        var eventType = typeof(T);

        if (handlers.ContainsKey(eventType))
        {
            var handlerList = handlers[eventType];

            foreach (var handler in handlerList)
            {
                (handler as Action<T>).Invoke(gameEvent);
            }
        }
    }

    public T GetPooledEvent<T>() where T : GameEvent
    {
        var eventType = typeof(T);
        T gameEvent;

        if (pools.ContainsKey(eventType) && (pools[eventType].Count > 0))
        {
            gameEvent = (T)pools[eventType][0];
            pools[eventType].RemoveAt(0);
        }
        else
        {
            gameEvent = (T)Activator.CreateInstance(eventType);
        }

        return gameEvent;
    }

    public void AddPooledEvent<T>(T gameEvent) where T : GameEvent
    {
        var eventType = typeof(T);

        if (!pools.ContainsKey(eventType))
        {
            pools[eventType] = new List<object>();
        }

        if (!pools[eventType].Contains(gameEvent))
        {
            pools[eventType].Add(gameEvent);
        }
    }
}
