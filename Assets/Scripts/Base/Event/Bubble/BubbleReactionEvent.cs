using Reaction;

public class BubbleReactionEvent : GameEvent
{
    public ReactionPriority priority;
    public Bubble bubble;

    public static void Dispatch(ReactionPriority priority, Bubble bubble)
    {
        BubbleReactionEvent gameEvent = GlobalState.Instance.EventDispatcher.GetPooledEvent<BubbleReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.bubble = bubble;

        GlobalState.Instance.EventDispatcher.DispatchPooled(gameEvent);
    }
}
