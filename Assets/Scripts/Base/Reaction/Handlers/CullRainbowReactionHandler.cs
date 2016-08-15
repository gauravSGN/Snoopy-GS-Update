using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.CullRainbow)]
    public class CullRainbowReactionHandler : BubbleReactionHandler
    {
        public CullRainbowReactionHandler(ReactionPriority priority) : base(priority) { }

        public override IEnumerator HandleActions()
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
