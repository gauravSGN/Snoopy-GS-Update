using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;
using System.Linq;
using Service;

namespace Reaction
{
    public class ReactionLogic : MonoBehaviour
    {
        public class ReactionHandlerFactory
            : AttributeDrivenFactory<ReactionHandler, ReactionHandlerAttribute, ReactionPriority>
        {
            override protected ReactionPriority GetKeyFromAttribute(ReactionHandlerAttribute attribute)
            {
                return attribute.Priority;
            }
        }

        private readonly SortedDictionary<ReactionPriority, ReactionHandler> handlers = new SortedDictionary<ReactionPriority, ReactionHandler>();

        protected void Start()
        {
            GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettledEvent>(OnBubbleSettled);
            GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleReactionEvent>(OnBubbleReactionEvent);

            var factory = new ReactionHandlerFactory();

            foreach (var priority in EnumExtensions.GetValues<ReactionPriority>())
            {
                var handler = factory.Create(priority);

                if (handler != null)
                {
                    handlers.Add(priority, handler);
                }
            }
        }

        private void OnBubbleReactionEvent(BubbleReactionEvent gameEvent)
        {
            ReactionHandler handler;

            if (handlers.TryGetValue(gameEvent.priority, out handler))
            {
                handler.Schedule(gameEvent);
            }
        }

        private void OnBubbleSettled(BubbleSettledEvent gameEvent)
        {
            StartCoroutine(ProcessReactions());
        }

        private IEnumerator ProcessReactions()
        {
            var active = true;

            while (active)
            {
                var coroutines = handlers.Where(p => p.Value.Count > 0).Select(p => p.Value.HandleActions()).ToList();
                active = coroutines.Count > 0;

                while (coroutines.Count > 0)
                {
                    if (coroutines[0].MoveNext())
                    {
                        yield return coroutines[0].Current;
                    }
                    else
                    {
                        coroutines.RemoveAt(0);
                    }
                }
            }

            if (GetComponent<Level>().levelState.remainingBubbles <= 0)
            {
                GlobalState.Instance.Services.Get<EventService>().Dispatch(new LevelCompleteEvent(false));
            }
            else
            {
                GlobalState.Instance.Services.Get<EventService>().Dispatch(new ReadyForNextBubbleEvent());
            }
        }
    }
}
