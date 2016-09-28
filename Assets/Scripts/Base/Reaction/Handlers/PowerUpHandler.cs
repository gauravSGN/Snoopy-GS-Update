using Graph;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    [ReactionHandlerAttribute(ReactionPriority.PowerUp)]
    public class PowerUpHandler : BubbleGroupReactionHandler
    {
        override protected IEnumerator Reaction()
        {
            var groups = scheduledGroups;
            scheduledGroups = new List<GroupReaction>();

            yield return null;

            foreach (var reaction in groups)
            {
                foreach (var bubble in reaction.bubbles)
                {
                    bubble.PopBubble();

                }
                // HACK: this is temporary just to unblock designers until I can figure out a true fix
                List<Bubble> toRemove = new List<Bubble>(reaction.bubbles);
                int idx = 0;
                while (idx < toRemove.Count)
                {
                    var bubble = toRemove[idx];
                    if (bubble.type == BubbleType.Cloud || bubble.type == BubbleType.Soap)
                    {
                        toRemove.RemoveAt(idx);
                    }
                    else
                    {
                        idx++;
                    }
                }
                GraphUtil.RemoveNodes(toRemove);

                yield return new WaitForSeconds(reaction.delay);
            }
        }
    }
}
