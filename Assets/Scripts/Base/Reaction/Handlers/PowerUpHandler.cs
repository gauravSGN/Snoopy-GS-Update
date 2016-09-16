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
                GraphUtil.RemoveNodes(reaction.bubbles);

                foreach (var bubble in reaction.bubbles)
                {
                    bubble.PopBubble();
                }

                yield return new WaitForSeconds(reaction.delay);
            }
        }
    }
}
