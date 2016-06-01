using UnityEngine;
using System;
using System.Collections.Generic;

public class Observable<T> where T : Observable<T>
{
    private List<Action<T>> listeners = new List<Action<T>>();

    public void AddListener(Action<T> action)
    {
        listeners.Add(action);
    }

    public void RemoveListener(Action<T> action)
    {
        if (listeners.Contains(action))
        {
            listeners.Remove(action);
        }
    }

    public void NotifyListeners()
    {
        foreach (var listener in listeners)
        {
            listener.Invoke((T)this);
        }
    }
}
