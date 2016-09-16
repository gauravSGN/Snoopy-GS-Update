using Event;
using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    abstract public class BubbleGroupReactionHandler : BaseReactionHandler<BubbleGroupReactionEvent>
    {
        sealed protected class GroupReaction
        {
            public List<Bubble> bubbles;
            public float delay;

            public GroupReaction(List<Bubble> bubbles, float delay)
            {
                this.bubbles = bubbles;
                this.delay = delay;
            }
        }

        protected List<GroupReaction> scheduledGroups = new List<GroupReaction>();

        override public int Count { get { return scheduledGroups.Count; } }

        override protected void OnReactionEvent(BubbleGroupReactionEvent gameEvent)
        {
            if (gameEvent.priority == priority)
            {
                scheduledGroups.Add(new GroupReaction(gameEvent.bubbles, gameEvent.delay));
            }
        }

        override protected IEnumerator PostReaction()
        {
            var baseReaction = base.PostReaction();

            while (baseReaction.MoveNext())
            {
                yield return null;
            }

            GlobalState.EventService.Dispatch<ScoreMultiplierCalloutEvent>(new ScoreMultiplierCalloutEvent());
        }
    }
}
