using System.Collections;
using System.Collections.Generic;
using Graph;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.PowerUp)]
    [ReactionHandlerAttribute(ReactionPriority.ChainPop)]
    [ReactionHandlerAttribute(ReactionPriority.TriggerDestroy)]
    public class GenericPopReactionHandler : ReactionHandler
    {
        public override IEnumerator HandleActions()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            GraphUtil.RemoveNodes(bubbles);

            foreach (var bubble in bubbles)
            {
                bubble.PopBubble();
            }
        }
    }
}
