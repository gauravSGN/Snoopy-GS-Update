using UnityEngine;
using Snoopy.Model;
using System.Collections.Generic;
using System.Linq;
using System;

public class BucketBubbleQueue : BaseBubbleQueue, BubbleQueue
{
    // drop to reserve bucket if bubble eliminated
    private BubbleQueueDefinition queueDefinition;
    private BubbleQueueDefinition.Bucket currentBucket;
    private int currentCount;
    private List<BubbleType> randomizedBucket;
    private System.Random random = new System.Random();
    private List<BubbleType> removedTypes = new List<BubbleType>();

    public BucketBubbleQueue(LevelState state, BubbleQueueDefinition definition) : base(state)
    {
        queueDefinition = definition;
        SetCurrentBucket();
        BuildQueue();
    }

    override protected BubbleType GenerateElement()
    {
        ChangeBucket();

        if (randomizedBucket.Count == 0)
        {
            for (int index = 0, length = currentBucket.counts.Length; index < length; index++)
            {
                for (int x = 0, count = currentBucket.counts[index]; x < count; x++)
                {
                    randomizedBucket.Add(LAUNCHER_BUBBLE_TYPES[index]);
                }
            }

            randomizedBucket = randomizedBucket.OrderBy(item => random.Next()).ToList();;
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
                Debug.Log("Removed : " + type);
                removedTypes.Add(type);
                var typeIndex = Array.IndexOf(LAUNCHER_BUBBLE_TYPES, type);
                // Debug.Log(typeIndex);

                currentBucket.counts[typeIndex] = 0;
                // Debug.Log(string.Join(",", currentBucket.counts.Select(item => item.ToString()).ToArray()));
                queueDefinition.extras.counts[typeIndex] = 0;
                // Debug.Log(string.Join(",", queueDefinition.extras.counts.Select(item => item.ToString()).ToArray()));
                queueDefinition.reserve.counts[typeIndex] = 0;
                // Debug.Log(string.Join(",", queueDefinition.reserve.counts.Select(item => item.ToString()).ToArray()));

                for (var index = 0; index < queueDefinition.buckets.Count; index++)
                {
                    queueDefinition.buckets[index].counts[typeIndex] = 0;
                    // Debug.Log(string.Join(",", queueDefinition.buckets[index].counts.Select(item => item.ToString()).ToArray()));
                }

                // Debug.Log("queued before elimination: " + string.Join(",", queued.Select(item => item.ToString()).ToArray()));
                for (var index = 0; index < queued.Count; index++)
                {
                    // Debug.Log("Before : " + index.ToString() + " : " + queued[index]);
                    if (queued[index] == type)
                    {
                        queued.RemoveAt(index);
                        index--;
                        currentCount--;
                    }
                }
            }

            randomizedBucket = new List<BubbleType>();
            // Debug.Log("queued after elimination : " + string.Join(",", queued.Select(item => item.ToString()).ToArray()));
        }
    }

    private void ChangeBucket()
    {
        if ((currentBucket != queueDefinition.extras) &&
            ((currentCount == currentBucket.length) || (currentBucket.counts.Sum() == 0)))
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
                    Debug.Log("Base Bucket");
                    returnBucket = queueDefinition.buckets[0];
                    queueDefinition.buckets.RemoveAt(0);
                }
                else
                {
                    Debug.Log("Reserve Bucket");
                    returnBucket = queueDefinition.reserve;
                    returnBucket.length = levelState.remainingBubbles;
                }
            }
            else
            {
                Debug.Log("Extras Bucket");
                returnBucket = queueDefinition.extras;
            }

            if ((returnBucket != queueDefinition.extras) && (returnBucket.counts.Sum() == 0))
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
}