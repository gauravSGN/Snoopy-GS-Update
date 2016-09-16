using Service;
using System.Collections;
using System.Collections.Generic;

namespace Reaction
{
    abstract public class BaseReactionHandler<EventType> : ReactionHandler
        where EventType : ReactionEvent
    {
        protected ReactionPriority priority;
        protected List<ReactionFunction> actions;

        private static BlockadeService blockade;

        protected delegate IEnumerator ReactionFunction();

        abstract public int Count { get; }
        abstract protected void OnReactionEvent(EventType gameEvent);
        abstract protected IEnumerator Reaction();

        virtual public void Setup(ReactionPriority priority)
        {
            this.priority = priority;
            actions = new List<ReactionFunction>() { Reaction, PostReaction };
            GlobalState.EventService.AddEventHandler<EventType>(OnReactionEvent);

            if (blockade == null)
            {
                blockade = GlobalState.Instance.Services.Get<BlockadeService>();
            }
        }

        public IEnumerator HandleActions()
        {
            foreach (var action in actions)
            {
                var reaction = action();

                while (reaction.MoveNext())
                {
                    yield return null;
                }
            }
        }

        virtual protected IEnumerator PostReaction()
        {
            while (blockade.ReactionsBlocked)
            {
                yield return null;
            }
        }
    }
}
