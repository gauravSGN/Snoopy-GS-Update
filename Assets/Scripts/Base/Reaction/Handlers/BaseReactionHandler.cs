using Service;
using System.Collections;

namespace Reaction
{
    abstract public class BaseReactionHandler<EventType> : ReactionHandler
        where EventType : ReactionEvent
    {
        protected ReactionPriority priority;
        abstract public int Count { get; }
        abstract public IEnumerator HandleActions();
        abstract protected void OnReactionEvent(EventType gameEvent);

        public BaseReactionHandler(ReactionPriority priority)
        {
            this.priority = priority;
            var eventService = GlobalState.Instance.Services.Get<EventService>();
            eventService.AddEventHandler<EventType>(OnReactionEvent);
        }
    }
}