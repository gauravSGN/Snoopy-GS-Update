using System.Collections;
using System.Collections.Generic;
using Graph;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.ChainPop)]
    public class ChainPopReactionHandler : ReactionHandler
    {
        public override IEnumerator HandleActions()
        {
            var bubbles = scheduledBubbles;
            scheduledBubbles = new List<Bubble>();

            yield return null;

            for (int index = 0, count = bubbles.Count; index < count; index++)
            {
                InitiateChainPop(bubbles[index]);
            }
        }

        private void InitiateChainPop(Bubble initialBubble)
        {
            var adjacentList = GetAdjacentBubbles(new List<Bubble>(), initialBubble);

            GraphUtil.RemoveNodes(adjacentList);

            for (int index = 0, length = adjacentList.Count; index < length; index++)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.GenericPop, adjacentList[index]);
            }
        }

        private List<Bubble> GetAdjacentBubbles(List<Bubble> adjacentList, Bubble current)
        {
            adjacentList.Add(current);
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
