using Snoopy.Model;
using UnityEngine;

public class BucketBubbleQueueTests : BubbleQueueTests
{
    override protected BubbleQueue GetBubbleQueue(LevelState levelState)
    {
        var definition = new BubbleQueueDefinition();

        for (var x = 1; x < 3; x++)
        {
            var bucket = new BubbleQueueDefinition.Bucket(1, 1, 1, 1, 1, 1);
            bucket.length = x * 5;
            definition.buckets.Add(bucket);
        }

        foreach (var item in definition.buckets)
        {
            Debug.Log(item.counts);
            Debug.Log(item.length);
        }
        definition.extras = new BubbleQueueDefinition.Bucket(1, 1, 1, 1, 1, 1);
        return new BucketBubbleQueue(levelState, definition);
    }
}