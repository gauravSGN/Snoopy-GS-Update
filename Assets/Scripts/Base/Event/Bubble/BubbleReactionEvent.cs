using System;

public class BubbleReactionEvent : GameEvent
{
    public ReactionPriority priority;
    public Action action;

    public static void Dispatch(ReactionPriority priority, Action action)
    {
        BubbleReactionEvent gameEvent = GlobalState.Instance.EventDispatcher.GetPooledEvent<BubbleReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.action = action;

        GlobalState.Instance.EventDispatcher.DispatchPooled(gameEvent);
    }
}
