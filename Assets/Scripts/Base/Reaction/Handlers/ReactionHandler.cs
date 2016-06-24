using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    public abstract class ReactionHandler
    {
        protected List<Bubble> scheduledBubbles = new List<Bubble>();

        public int Count { get { return scheduledBubbles.Count; } }

        public void Schedule(BubbleReactionEvent gameEvent)
        {
            scheduledBubbles.Add(gameEvent.bubble);
        }

        public abstract IEnumerator HandleActions();
    }
}
