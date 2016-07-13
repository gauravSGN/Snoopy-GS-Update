using NUnit.Framework;
using System.Collections.Generic;

abstract public class BubbleQueueTests
{
    private BubbleQueue bubbleQueue;
    private readonly int[] invalidIndexes =
    {
        -1,
        -1000,
        BaseBubbleQueue.MAX_QUEUE_SIZE,
        BaseBubbleQueue.MAX_QUEUE_SIZE + 1000,
    };

    abstract protected BubbleQueue GetBubbleQueue(LevelState levelState);

    [SetUp]
    public void Init()
    {
        var levelState = new LevelState();

        levelState.UpdateTypeTotals(BubbleType.Blue, 100);
        levelState.UpdateTypeTotals(BubbleType.Red, 100);
        levelState.UpdateTypeTotals(BubbleType.Yellow, 100);
        levelState.UpdateTypeTotals(BubbleType.Green, 100);
        levelState.UpdateTypeTotals(BubbleType.Pink, 100);
        levelState.UpdateTypeTotals(BubbleType.Purple, 100);

        bubbleQueue = GetBubbleQueue(levelState);
    }

    [Test]
    public void GetNext_EmptyQueue()
    {
        Assert.Contains(bubbleQueue.GetNext(), BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES);
    }

    [Test]
    public void GetNext_FilledQueue()
    {
        Assert.Contains(bubbleQueue.Peek(BaseBubbleQueue.MAX_QUEUE_SIZE - 1), BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES);

        for (var i = 0; i < BaseBubbleQueue.MAX_QUEUE_SIZE; i++)
        {
            Assert.Contains(bubbleQueue.GetNext(), BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES);
        }
    }

    [Test]
    public void Peek_InvalidIndexes()
    {
        foreach (var invalidIndex in invalidIndexes)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => bubbleQueue.Peek(invalidIndex));
        }
    }

    [Test]
    public void Peek_ValidIndexes()
    {
        for (var i = 0; i < BaseBubbleQueue.MAX_QUEUE_SIZE; i++)
        {
            Assert.Contains(bubbleQueue.Peek(i), BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES);
        }
    }

    [Test]
    public void Rotate_InvalidIndexes()
    {
        foreach (var invalidIndex in invalidIndexes)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => bubbleQueue.Rotate(invalidIndex));
        }
    }

    [Test]
    public void Rotate_ValidIndexes()
    {
        var bubbleTypeList = new List<BubbleType>();

        for (var numItems = 2; numItems <= BaseBubbleQueue.MAX_QUEUE_SIZE; numItems++)
        {
            bubbleTypeList.Clear();

            for (var i = 0; i < numItems; i++)
            {
                bubbleTypeList.Add(bubbleQueue.Peek(i));
            }

            bubbleQueue.Rotate(numItems);

            for (var i = 0; i < numItems; i++)
            {
                var newIndex = (i == 0) ? (numItems - 1) : (i - 1);
                Assert.AreEqual(bubbleTypeList[i], bubbleQueue.Peek(newIndex));
            }
        }
    }
}
