using NUnit.Framework;
using System.Collections.Generic;

public class BubbleQueueTests
{
    private BubbleQueue bubbleQueue;
    private int[] invalidIndexes =
    {
        -1,
        -1000,
        BubbleQueue.MAX_QUEUE_SIZE,
        BubbleQueue.MAX_QUEUE_SIZE + 1000,
    };

    [SetUp]
    public void Init()
    {
        bubbleQueue = new BubbleQueue();
    }

    [Test]
    public void GetNext_EmptyQueue()
    {
        Assert.Contains(bubbleQueue.GetNext(), BubbleQueue.LAUNCHER_BUBBLE_TYPES);
    }

    [Test]
    public void GetNext_FilledQueue()
    {
        Assert.Contains(bubbleQueue.Peek(BubbleQueue.MAX_QUEUE_SIZE - 1), BubbleQueue.LAUNCHER_BUBBLE_TYPES);

        for (var i = 0; i < BubbleQueue.MAX_QUEUE_SIZE; i++)
        {
            Assert.Contains(bubbleQueue.GetNext(), BubbleQueue.LAUNCHER_BUBBLE_TYPES);
        }
    }

    [Test]
    public void Peek_InvalidIndexes()
    {
        foreach (var invalidIndex in invalidIndexes)
        {
            Assert.Throws<System.IndexOutOfRangeException>(() => bubbleQueue.Peek(invalidIndex));
        }
    }

    [Test]
    public void Peek_ValidIndexes()
    {
        for (var i = 0; i < BubbleQueue.MAX_QUEUE_SIZE; i++)
        {
            Assert.Contains(bubbleQueue.Peek(i), BubbleQueue.LAUNCHER_BUBBLE_TYPES);
        }
    }

    [Test]
    public void Rotate_InvalidIndexes()
    {
        foreach (var invalidIndex in invalidIndexes)
        {
            Assert.Throws<System.IndexOutOfRangeException>(() => bubbleQueue.Rotate(invalidIndex));
        }
    }

    [Test]
    public void Rotate_ValidIndexes()
    {
        var bubbleTypeList = new List<BubbleType>();

        for (var numItems = 2; numItems < BubbleQueue.MAX_QUEUE_SIZE + 1; numItems++)
        {
            bubbleQueue = new BubbleQueue();
            bubbleTypeList.Clear();

            for (var i = 0; i < numItems; i++ )
            {
                bubbleTypeList.Add(bubbleQueue.Peek(i));
            }

            bubbleQueue.Rotate(numItems - 1);

            for (var i = 0; i < numItems; i++)
            {
                var newIndex = (i == 0) ? (numItems - 1) : (i - 1);
                Assert.AreEqual(bubbleTypeList[i], bubbleQueue.Peek(newIndex));
            }
        }
    }
}
