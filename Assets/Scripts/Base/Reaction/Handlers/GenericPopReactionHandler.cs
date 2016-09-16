using Graph;
using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.GenericPop)]
    [ReactionHandlerAttribute(ReactionPriority.PhysicsDestroy)]
    public class GenericPopReactionHandler : BubbleReactionHandler
    {
        override protected IEnumerator Reaction()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            for (int index = 0, count = bubbles.Count; index < count; index++)
            {
                bubbles[index].PopBubble();
            }

            GraphUtil.RemoveNodes(bubbles);
        }
    }
}
