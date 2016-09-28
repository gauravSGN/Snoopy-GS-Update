using System.Collections;
using System.Collections.Generic;
using Graph;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.ChainPop)]
    public class ChainPopReactionHandler : BubbleReactionHandler
    {
        override protected IEnumerator Reaction()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            GraphUtil.RemoveNodes(bubbles);

            for (int index = 0, length = bubbles.Count; index < length; index++)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.GenericPop, bubbles[index]);
            }
        }

        override protected void OnReactionEvent(BubbleReactionEvent gameEvent)
        {
            if (gameEvent.priority == priority)
            {
                GetAdjacentBubbles(scheduledBubbles, gameEvent.bubble);
            }
        }

        private List<Bubble> GetAdjacentBubbles(List<Bubble> adjacentList, Bubble current)
        {
            if (!adjacentList.Contains(current))
            {
                adjacentList.Add(current);
            }

            foreach (Bubble neighbor in current.Neighbors)
            {
                if ((neighbor.type == current.type) && !adjacentList.Contains(neighbor))
                {
                    GetAdjacentBubbles(adjacentList, neighbor);
                }
            }

            return adjacentList;
        }
    }
}
