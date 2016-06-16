using System;
using System.Collections.Generic;

public class Observable
{
    private readonly List<Action<Observable>> listeners = new List<Action<Observable>>();

    public void AddListener(Action<Observable> action)
    {
        listeners.Add(action);
    }

    public void RemoveListener(Action<Observable> action)
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
            listener.Invoke(this);
        }
    }
}
