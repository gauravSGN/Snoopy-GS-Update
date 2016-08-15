using System.Collections.Generic;

namespace Reaction
{
    abstract public class BubbleReactionHandler : BaseReactionHandler<BubbleReactionEvent>
    {
        protected List<Bubble> scheduledBubbles = new List<Bubble>();

        override public int Count { get { return scheduledBubbles.Count; } }

        public BubbleReactionHandler(ReactionPriority priority) : base(priority) { }

        override protected void OnReactionEvent(BubbleReactionEvent gameEvent)
        {
            if (gameEvent.priority == priority)
            {
                scheduledBubbles.Add(gameEvent.bubble);
            }
        }
    }
}
