using System.Collections;
using System.Collections.Generic;
using Graph;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.PowerUp)]
    [ReactionHandlerAttribute(ReactionPriority.GenericPop)]
    [ReactionHandlerAttribute(ReactionPriority.PhysicsDestroy)]
    public class GenericPopReactionHandler : BubbleReactionHandler
    {
        public override IEnumerator HandleActions()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            GraphUtil.RemoveNodes(bubbles);

            for (int index = 0, count = bubbles.Count; index < count; index++)
            {
                bubbles[index].PopBubble();
            }
        }
    }
}
