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

            foreach (var groupReaction in groups)
            {
                GraphUtil.RemoveNodes(groupReaction.bubbles);

                for (int index = 0, count = groupReaction.bubbles.Count; index < count; index++)
                {
                    groupReaction.bubbles[index].PopBubble();
                }

                yield return new WaitForSeconds(groupReaction.delay);
            }
        }
    }
}
