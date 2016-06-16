using System;
using System.Collections.Generic;

public class Observable
{
    private readonly List<object> listeners = new List<object>();

    public void AddListener(object action)
    {
        listeners.Add(action);
    }

    public void AddListener<T>(Action<T> action) where T : Observable
    {
        AddListener((object)action);
    }

    public void RemoveListener(object action)
    {
        if (listeners.Contains(action))
        {
            listeners.Remove(action);
        }
    }

    public void RemoveListener<T>(Action<T> action) where T : Observable
    {
        RemoveListener((object)action);
    }

    public void NotifyListeners()
    {
        foreach (var listener in listeners)
        {
            var action = (Action<Observable>)listener;
            action.Invoke(this);
        }
    }
}
