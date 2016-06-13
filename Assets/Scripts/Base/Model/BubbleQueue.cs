using System;
using System.Collections.Generic;

public class BubbleQueue
{
    public const int MAX_QUEUE_SIZE = 4;
    public static readonly BubbleType[] LAUNCHER_BUBBLE_TYPES =
    {
        BubbleType.Blue,
        BubbleType.Yellow,
        BubbleType.Red,
        BubbleType.Green,
    };

    private readonly List<BubbleType> queued = new List<BubbleType>();

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
        VerifyIndex(index);

        while (queued.Count <= index)
        {
            queued.Add(GenerateElement());
        }

        return queued[index];
    }

    public void Rotate(int index)
    {
        VerifyIndex(index);

        Peek(index);
        var first = GetNext();

        queued.Insert(index, first);
    }

    private BubbleType GenerateElement()
    {
        return LAUNCHER_BUBBLE_TYPES[UnityEngine.Random.Range(0, LAUNCHER_BUBBLE_TYPES.Length)];
    }

    private void VerifyIndex(int index)
    {
        if ((index < 0) || (index > (MAX_QUEUE_SIZE - 1)))
        {
            throw new IndexOutOfRangeException(index.ToString());
        }
    }
}
