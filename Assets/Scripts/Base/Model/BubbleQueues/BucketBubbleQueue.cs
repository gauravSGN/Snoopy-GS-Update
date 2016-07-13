using UnityEngine;
using Snoopy.Model;
using System.Collections.Generic;
using System.Linq;

public class BucketBubbleQueue : BaseBubbleQueue, BubbleQueue
{
    // drop to reserve bucket if bubble eliminated
    // drop to extra moves bucket if past end of level
    private BubbleQueueDefinition queueData;
    private BubbleQueueDefinition.Bucket currentBucket;
    private int currentCount;
    private List<BubbleType> randomizedBucket;
    private System.Random random = new System.Random();

    public BucketBubbleQueue(LevelState state, BubbleQueueDefinition definition) : base(state)
    {
        queueData = definition;
        SetCurrentBucket();
        BuildQueue();
    }

    override protected BubbleType GenerateElement()
    {
        if ((currentBucket != queueData.extras) && (currentCount == currentBucket.length))
        {
            SetCurrentBucket();
        }

        if (randomizedBucket.Count == 0)
        {
            var temp = new List<BubbleType>();

            for (int index = 0, length = currentBucket.counts.Length; index < length; index++)
            {
                for (int x = 0, count = currentBucket.counts[index]; x < count; x++)
                {
                    Debug.Log(index + " : " + LAUNCHER_BUBBLE_TYPES[index]);
                    temp.Add(LAUNCHER_BUBBLE_TYPES[index]);
                }
            }

            randomizedBucket = temp.OrderBy(item => random.Next()).ToList();;
        }

        Debug.Log(string.Join(",", randomizedBucket.Select(item => item.ToString()).ToArray()));

        currentCount++;
        var nextElement = randomizedBucket[0];
        randomizedBucket.RemoveAt(0);
        return nextElement;
    }

    override protected void RemoveInactiveTypes()
    {
        var typeTotals = levelState.typeTotals;


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

        if ((queueData.extras != currentBucket) && (levelState.remainingBubbles != 0))
        {
            if (queueData.buckets.Count > 0)
            {
                Debug.Log("Base Bucket");
                returnBucket = queueData.buckets[0];
                queueData.buckets.RemoveAt(0);
            }
            else
            {
                Debug.Log("Reserve Bucket");
                returnBucket = queueData.reserve;
            }
        }
        else
        {
            Debug.Log("Extras Bucket");
            returnBucket = queueData.extras;
        }

        return returnBucket;
    }
}