using System;
using System.Collections.Generic;
using Core;

public class PriorityQueue<T, U>
{
    private List<Tuple<T, U>> items;
    private Func<U, U, int> comparator;

    public PriorityQueue(Func<U, U, int> comparator, int capacity = 1)
    {
        items = new List<Tuple<T, U>>(capacity);
        this.comparator = comparator;
    }

    public int Count
    {
        get{ return items.Count; }
    }

    public void Clear()
    {
        items.Clear();
    }

    public bool Contains(Tuple<T, U> item)
    {
        return items.Contains(item);
    }

    public T Dequeue()
    {
        if (items.Count > 0)
        {
            var toReturn = items[0].Item1;
            items.RemoveAt(0);
            return toReturn;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void Enqueue(Tuple<T, U> item)
    {
        int index = 0;
        for (int length = items.Count; index < length; ++index)
        {
            if (comparator(item.Item2, items[index].Item2) < 0)
            {
                break;
            }
        }
        items.Insert(index, item);
    }

    public void Enqueue(T item, U priority)
    {
        Enqueue(new Tuple<T,U>(item, priority));
    }

    public T Peek()
    {
        if (items.Count > 0)
        {
            return items[0].Item1;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public IEnumerator<Tuple<T, U>> GetEnumerator()
    {
        return items.GetEnumerator();
    }
}
