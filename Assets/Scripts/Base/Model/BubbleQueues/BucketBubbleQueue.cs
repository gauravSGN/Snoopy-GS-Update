using Model;
using Snoopy.Model;
using System.Collections.Generic;
using System.Linq;
using System;

public class BucketBubbleQueue : BaseBubbleQueue, BubbleQueue
{
    private readonly BubbleQueueDefinition queueDefinition;
    private readonly List<BubbleType> removedTypes = new List<BubbleType>();

    private BubbleQueueDefinition.Bucket currentBucket;
    private RandomBag<BubbleType> bag = new RandomBag<BubbleType>();
    private int currentCount;

    public BucketBubbleQueue(LevelState state, BubbleQueueDefinition definition) : base(state)
    {
        queueDefinition = definition;
        SetCurrentBucket();
        RemoveTypes(GetEliminatedTypes());
        BuildQueue();
    }

    override protected BubbleType GenerateElement()
    {
        ChangeBucket();

        if (bag.Empty)
        {
            RefillBag();
        }

        currentCount++;

        return bag.Next();
    }

    override protected void RemoveInactiveTypes()
    {
        var eliminatedTypes = GetEliminatedTypes();

        if (eliminatedTypes.Count > 0)
        {
            RemoveTypes(eliminatedTypes);
            bag.Clear();

            if ((currentBucket != queueDefinition.reserve) &&
                (currentBucket.counts.Where( x => x > 0 ).ToList().Count < 3))
            {
                var cycleLength = currentBucket.counts.Sum();

                if (currentBucket.mandatory && (currentCount <= cycleLength))
                {
                    currentBucket.length = cycleLength;
                }
                else
                {
                    SetCurrentBucket();
                }
            }
        }
    }

    private void RemoveTypes(List<BubbleType> eliminatedTypes)
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
    }

    private void ChangeBucket()
    {
        if (currentBucket != queueDefinition.extras)
        {
            var possibleColorCount = currentBucket.counts.Where( x => x > 0 ).ToList().Count;

            if ((currentCount == currentBucket.length) || (possibleColorCount == 0))
            {
                SetCurrentBucket();
            }
        }
    }

    private void SetCurrentBucket()
    {
        currentBucket = GetNextBucket();
        currentCount = 0;
        bag.Clear();
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

    private List<BubbleType> GetEliminatedTypes()
    {
        return levelState.typeTotals.Where(IsEliminated).Select(pair => pair.Key).ToList();
    }

    private bool IsEliminated(KeyValuePair<BubbleType, int> pair)
    {
        return LAUNCHER_BUBBLE_TYPES.Contains(pair.Key) && (pair.Value == 0) && (!removedTypes.Contains(pair.Key));
    }

    private void RefillBag()
    {
        if (currentBucket.counts.Sum() > 0)
        {
            for (int index = 0, length = currentBucket.counts.Length; index < length; index++)
            {
                bag.Add(LAUNCHER_BUBBLE_TYPES[index], currentBucket.counts[index]);
            }
        }
        else
        {
            bag.Add(BubbleType.Blue);
        }
    }
}
