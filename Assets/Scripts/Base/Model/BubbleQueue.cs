using UnityEngine;
using System.Collections.Generic;

public class BubbleQueue
{
    private List<BubbleType> queued = new List<BubbleType>();

    public BubbleType GetNext()
    {
        BubbleType next;

        if (queued.Count == 0)
        {
            next = GenerateElement();
        }
        else
        {
            next = queued[0];
            queued.RemoveAt(0);
        }

        return next;
    }

    public BubbleType Peek(int index)
    {
        while (queued.Count <= index)
        {
            queued.Add(GenerateElement());
        }

        return queued[index];
    }

    public void RotateQueue(int depth)
    {
        if (depth > 0)
        {
            Peek(depth - 1);
            var first = GetNext();

            queued.Insert(depth - 1, first);
        }
    }

    private BubbleType GenerateElement()
    {
        return (BubbleType)Random.Range(0, 4);
    }
}
