using System;
using System.Collections.Generic;

public class EventDispatcher
{
    private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();
    private readonly Dictionary<Type, List<object>> pools = new Dictionary<Type, List<object>>();

    public void Reset()
    {
        handlers.Clear();
    }

    public void AddEventHandler<T>(Action<T> handler) where T : GameEvent
    {
        DictionaryInsert(handlers, typeof(T), handler);
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

    public void DispatchPooled<T>(T gameEvent) where T : GameEvent
    {
        Dispatch(gameEvent);
        AddPooledEvent(gameEvent);
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
        DictionaryInsert(pools, typeof(T), gameEvent);
    }

    private void DictionaryInsert<K, V>(Dictionary<K, List<V>> dictionary, K key, V item)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, new List<V>());
        }

        var list = dictionary[key];
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }
}
