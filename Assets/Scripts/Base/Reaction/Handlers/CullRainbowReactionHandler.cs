using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.CullRainbow)]
    public class CullRainbowReactionHandler : BubbleReactionHandler
    {
        override protected IEnumerator Reaction()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            foreach (var bubble in bubbles)
            {
                if (bubble.NumberOfNeighbors != 0)
                {
                    bubble.RemoveFromGraph();
                }
            }
        }
    }
}
