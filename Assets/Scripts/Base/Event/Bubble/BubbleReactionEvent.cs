using UnityEngine;
using System;

public class BubbleReactionEvent : GameEvent
{
    public ReactionPriority priority;
    public Action action;

    public static void Dispatch(ReactionPriority priority, Action action)
    {
        BubbleReactionEvent gameEvent = EventDispatcher.Instance.GetPooledEvent<BubbleReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.action = action;

        EventDispatcher.Instance.Dispatch(gameEvent);
    }
}
