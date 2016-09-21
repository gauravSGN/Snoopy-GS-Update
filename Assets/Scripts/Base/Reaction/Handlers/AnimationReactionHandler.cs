using Effects;
using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.PreReactionAnimation)]
    public class AnimationReactionHandler : BaseReactionHandler<AnimationReactionEvent>
    {
        private List<IEnumerator> scheduledAnimations = new List<IEnumerator>();

        override public int Count { get { return scheduledAnimations.Count; } }

        override protected IEnumerator Reaction()
        {
            var playingAnimations = scheduledAnimations;
            scheduledAnimations = new List<IEnumerator>();

            var index = 0;

            while (index < playingAnimations.Count)
            {
                if (!playingAnimations[index].MoveNext())
                {
                    index++;
                }

                yield return null;
            }
        }

        override protected void OnReactionEvent(AnimationReactionEvent gameEvent)
        {
            if (gameEvent.priority == priority)
            {
                scheduledAnimations.Add(AnimationEffect.PlayBlocking(gameEvent.gameObject, gameEvent.animationType));
            }
        }
    }
}
