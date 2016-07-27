using Snoopy.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class BucketBubbleQueue : BaseBubbleQueue, BubbleQueue
{
    private readonly BubbleQueueDefinition queueDefinition;
    private readonly System.Random random = new System.Random();
    private readonly List<BubbleType> removedTypes = new List<BubbleType>();

    private BubbleQueueDefinition.Bucket currentBucket;
    private int currentCount;
    private List<BubbleType> randomizedBucket;

    public BucketBubbleQueue(LevelState state, BubbleQueueDefinition definition) : base(state)
    {
        queueDefinition = definition;
        SetCurrentBucket();
        RemoveInactiveTypes();
        BuildQueue();
    }

    override protected BubbleType GenerateElement()
    {
        ChangeBucket();

        if (randomizedBucket.Count == 0)
        {
            RefillRandomizedBucket();
        }

        currentCount++;
        var nextElement = randomizedBucket[0];
        randomizedBucket.RemoveAt(0);
        return nextElement;
    }

    override protected void RemoveInactiveTypes()
    {
        var typeTotals = levelState.typeTotals;
        var eliminatedTypes = typeTotals.Where(IsEliminated).Select(pair => pair.Key).ToList();

        if (eliminatedTypes.Count > 0)
        {
            foreach (var type in eliminatedTypes)
            {
                removedTypes.Add(type);
                var typeIndex = Array.IndexOf(LAUNCHER_BUBBLE_TYPES, type);

                currentBucket.counts[typeIndex] = 0;
                queueDefinition.extras.counts[typeIndex] = 0;
                queueDefinition.reserve.counts[typeIndex] = 0;

                for (var index = 0; index < queueDefinition.buckets.Count; index++)
                {
                    queueDefinition.buckets[index].counts[typeIndex] = 0;
                }

                var initialCount = queued.Count;
                while (queued.Remove(type));

                currentCount -= (initialCount - queued.Count);
            }

            randomizedBucket = new List<BubbleType>();
        }
    }

    private void ChangeBucket()
    {
        var changeBucket = false;

        if (currentBucket != queueDefinition.extras)
        {
            var possibleColorCount = currentBucket.counts.Where( x => x > 0 ).ToList().Count;

            if ((currentCount == currentBucket.length) || (possibleColorCount == 0))
            {
                changeBucket = true;
            }
            else if ((currentBucket != queueDefinition.reserve) && (possibleColorCount < 3))
            {
                var cycleLength = currentBucket.counts.Sum();

                if (currentBucket.mandatory && (currentCount <= cycleLength))
                {
                    currentBucket.length = cycleLength;
                }
                else
                {
                    changeBucket = true;
                }
            }
        }

        if (changeBucket)
        {
            SetCurrentBucket();
        }
    }

    private void SetCurrentBucket()
    {
        currentBucket = GetNextBucket();
        currentCount = 0;
        randomizedBucket = new List<BubbleType>();
    }

    private BubbleQueueDefinition.Bucket GetNextBucket()
    {
        BubbleQueueDefinition.Bucket returnBucket = null;

        while (returnBucket == null)
        {
            if ((queueDefinition.extras != currentBucket) && (levelState.remainingBubbles > 0))
            {
                if (queueDefinition.buckets.Count > 0)
                {
                    returnBucket = queueDefinition.buckets[0];
                    queueDefinition.buckets.RemoveAt(0);
                }
                else
                {
                    returnBucket = queueDefinition.reserve;
                    returnBucket.length = levelState.remainingBubbles;
                }
            }
            else
            {
                returnBucket = queueDefinition.extras;
            }

            if ((returnBucket != queueDefinition.extras)  &&
                (returnBucket != queueDefinition.reserve) &&
                (returnBucket.counts.Sum() == 0))
            {
                returnBucket = null;
            }
        }

        return returnBucket;
    }

    private bool IsEliminated(KeyValuePair<BubbleType, int> pair)
    {
        return LAUNCHER_BUBBLE_TYPES.Contains(pair.Key) && (pair.Value == 0) && (!removedTypes.Contains(pair.Key));
    }

    private void RefillRandomizedBucket()
    {
        if (currentBucket.counts.Sum() > 0)
        {
            for (int index = 0, length = currentBucket.counts.Length; index < length; index++)
            {
                for (int x = 0, count = currentBucket.counts[index]; x < count; x++)
                {
                    randomizedBucket.Add(LAUNCHER_BUBBLE_TYPES[index]);
                }
            }

            randomizedBucket = randomizedBucket.OrderBy(item => random.Next()).ToList();
        }
        else
        {
            randomizedBucket.Add(BubbleType.Blue);
        }
    }
}
