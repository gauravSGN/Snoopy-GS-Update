using Reaction;

public class BubbleReactionEvent : ReactionEvent
{
    public Bubble bubble;

    public static void Dispatch(ReactionPriority priority, Bubble bubble)
    {
        var gameEvent = GlobalState.EventService.GetPooledEvent<BubbleReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.bubble = bubble;

        GlobalState.EventService.DispatchPooled(gameEvent);
    }
}
