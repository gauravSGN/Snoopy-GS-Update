﻿using NUnit.Framework;
using System.Collections.Generic;

public class BubbleQueueTests
{
    private BubbleQueue bubbleQueue;
    private readonly int[] invalidIndexes =
    {
        -1,
        -1000,
        BubbleQueue.MAX_QUEUE_SIZE,
        BubbleQueue.MAX_QUEUE_SIZE + 1000,
    };

    [SetUp]
    public void Init()
    {
        var levelState = new LevelState();

        levelState.UpdateTypeTotals(BubbleType.Blue, 100);
        levelState.UpdateTypeTotals(BubbleType.Red, 100);
        levelState.UpdateTypeTotals(BubbleType.Yellow, 100);
        levelState.UpdateTypeTotals(BubbleType.Green, 100);

        bubbleQueue = new BubbleQueue(levelState);
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
            Assert.Throws<System.ArgumentOutOfRangeException>(() => bubbleQueue.Peek(invalidIndex));
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
            Assert.Throws<System.ArgumentOutOfRangeException>(() => bubbleQueue.Rotate(invalidIndex));
        }
    }

    [Test]
    public void Rotate_ValidIndexes()
    {
        var bubbleTypeList = new List<BubbleType>();

        for (var numItems = 2; numItems <= BubbleQueue.MAX_QUEUE_SIZE; numItems++)
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
