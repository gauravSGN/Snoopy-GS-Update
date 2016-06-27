using System.Collections;
using System.Collections.Generic;
using Graph;
using UnityEngine;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.Pop)]
    public class PopReactionHandler : ReactionHandler
    {
        public override IEnumerator HandleActions()
        {
            var bubbleList = new List<Bubble>(scheduledBubbles);
            var chains = BuildPopChains(scheduledBubbles);
            var delay = GlobalState.Instance.Config.reactions.popSpreadDelay;

            scheduledBubbles = new List<Bubble>();
            GraphUtil.RemoveNodes(bubbleList);

            yield return null;

            while (chains.Count > 0)
            {
                var chain = chains[0];
                chains.RemoveAt(0);

                var count = chain.Count;
                for (var index = 0; index < count; index++)
                {
                    chain[index].PopBubble();
                }

                yield return new WaitForSeconds(delay);
            }
        }

        private List<List<Bubble>> BuildPopChains(List<Bubble> bubbles)
        {
            var chains = new List<List<Bubble>>();
            chains.Add(new List<Bubble>());

            while (bubbles.Count > 0)
            {
                chains[0].Add(bubbles[0]);
                bubbles.RemoveAt(0);

                BuildChain(chains, bubbles, 0, chains[0].Count - 1);
            }

            return chains;
        }

        private void BuildChain(List<List<Bubble>> chains, List<Bubble> bubbles, int depth, int start)
        {
            if (chains.Count <= (depth + 1))
            {
                chains.Add(new List<Bubble>());
            }

            var count = chains[depth].Count;
            var begin = chains[depth + 1].Count;

            for (var index = start; index < count; index++)
            {
                var bubble = chains[depth][index];

                foreach (Bubble neighbor in bubble.Neighbors)
                {
                    if (bubbles.Contains(neighbor))
                    {
                        chains[depth + 1].Add(neighbor);
                        bubbles.Remove(neighbor);
                    }
                }
            }

            if (chains[depth + 1].Count > begin)
            {
                BuildChain(chains, bubbles, depth + 1, begin);
            }
        }
    }
}
