using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.Cull)]
    public class CullReactionHandler : BubbleReactionHandler
    {
        public override IEnumerator HandleActions()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            foreach (var bubble in bubbles)
            {
                bubble.MakeBubbleFall();
            }
        }
    }
}
