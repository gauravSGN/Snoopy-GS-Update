using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class PriorityQueueTests
{
    readonly private int[] values =  { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private PriorityQueue<int, int> queue;

    static public int compareInts(int val1, int val2)
    {
        return (val1 - val2) - (val2 - val1);
    }

    [SetUp]
    public void Init()
    {
        queue = new PriorityQueue<int, int>(compareInts, 10);
    }

    [Test]
    public void DequeEmpty()
    {
        Assert.Throws<System.InvalidOperationException>(() => queue.Dequeue());
    }

    [Test]
    public void PeekEmpty()
    {
        Assert.Throws<System.InvalidOperationException>(() => queue.Peek());
    }

    [Test]
    public void EnqueueAll()
    {
        EnqueueInOrder();
        Assert.AreEqual(values.Length, queue.Count);
    }

    [Test]
    public void DequeueOrder()
    {
        queue.Clear();
        EnqueueInOrder();
        int idx = 0;
        Assert.AreEqual(values.Length, queue.Count);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            Assert.AreEqual(values[idx], item);
            idx++;
        }
    }

    [Test]
    public void DequeueReverseOrder()
    {
        queue.Clear();
        EnqueueReverseOrder();
        int idx = values.Length - 1;
        Assert.AreEqual(values.Length, queue.Count);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            Assert.AreEqual(values[idx], item);
            idx--;
        }
    }

    [Test]
    public void Clear()
    {
        EnqueueInOrder();
        queue.Clear();
        Assert.AreEqual(0, queue.Count);
    }

    private void EnqueueInOrder()
    {
        for (int i = 0; i < values.Length; ++i)
        {
            queue.Enqueue(values[i], values[i]);
        }
    }

    private void EnqueueReverseOrder()
    {
        for (int i = 0, prio = values.Length; i < values.Length; ++i, --prio)
        {
            queue.Enqueue(values[i], prio);
        }
    }
}
