using Reaction;
using Service;

public class BubbleReactionEvent : GameEvent
{
    public ReactionPriority priority;
    public Bubble bubble;

    public static void Dispatch(ReactionPriority priority, Bubble bubble)
    {
        BubbleReactionEvent gameEvent = GlobalState.Instance.Services.Get<EventService>().GetPooledEvent<BubbleReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.bubble = bubble;

        GlobalState.Instance.Services.Get<EventService>().DispatchPooled(gameEvent);
    }
}
