using System;
using System.Collections.Generic;
using Snoopy.Model;

public static class BubbleQueueFactory
{
    private static Dictionary<BubbleQueueType, Func<QueueData, BubbleQueue>> typeMap =
        new Dictionary<BubbleQueueType, Func<QueueData, BubbleQueue>>()
        {
            { BubbleQueueType.WeightedBubbleQueue, WeightedCreator },
            { BubbleQueueType.BucketBubbleQueue, BucketCreator },
        };

    private struct QueueData
    {
        public LevelState levelState;
        public BubbleQueueDefinition bubbleQueueDefinition;

        public QueueData(LevelState state, BubbleQueueDefinition definition)
        {
            levelState = state;
            bubbleQueueDefinition = definition;
        }
    }

    public static BubbleQueue GetBubbleQueue(BubbleQueueType queueType,
                                             LevelState levelState,
                                             BubbleQueueDefinition definition)
    {
        return typeMap[queueType](new QueueData(levelState, definition));
    }

    private static BubbleQueue WeightedCreator(QueueData queueData)
    {
        return new WeightedBubbleQueue(queueData.levelState);
    }

    private static BubbleQueue BucketCreator(QueueData queueData)
    {
        return new BucketBubbleQueue(queueData.levelState, queueData.bubbleQueueDefinition);
    }
}