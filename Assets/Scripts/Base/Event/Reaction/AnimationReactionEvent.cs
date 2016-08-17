using Reaction;
using Animation;
using UnityEngine;

public class AnimationReactionEvent : ReactionEvent
{
    public AnimationType animationType;
    public GameObject gameObject;

    public static void Dispatch(ReactionPriority priority, AnimationType animationType, GameObject gameObject)
    {
        var gameEvent = GlobalState.EventService.GetPooledEvent<AnimationReactionEvent>();

        gameEvent.priority = priority;
        gameEvent.gameObject = gameObject;
        gameEvent.animationType = animationType;

        GlobalState.EventService.DispatchPooled(gameEvent);
    }
}
