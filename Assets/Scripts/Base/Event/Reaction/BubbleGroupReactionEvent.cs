using Reaction;
using System.Collections.Generic;

public class BubbleGroupReactionEvent : ReactionEvent
{
    public List<Bubble> bubbles;
    public float delay;

    public static void Dispatch(ReactionPriority priority, List<Bubble> bubbles, float delay)
    {
        var gameEvent = GlobalState.EventService.GetPooledEvent<BubbleGroupReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.bubbles = bubbles;
        gameEvent.delay = delay;

        GlobalState.EventService.DispatchPooled(gameEvent);
    }
}
