using Util;
using System.Linq;
using UnityEngine;
using ExtensionMethods;
using System.Collections;
using HandlerDict = System.Collections.Generic.SortedDictionary<Reaction.ReactionPriority, Reaction.ReactionHandler>;

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

        private readonly HandlerDict handlers = new HandlerDict();

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<BubbleSettledEvent>(OnBubbleSettled);

            var factory = new ReactionHandlerFactory();

            foreach (var priority in EnumExtensions.GetValues<ReactionPriority>())
            {
                var handler = factory.Create(priority);

                if (handler != null)
                {
                    handler.Setup(priority);
                    handlers.Add(priority, handler);
                }
            }
        }

        private void OnBubbleSettled(BubbleSettledEvent gameEvent)
        {
            StartCoroutine(ProcessReactions());
        }

        private IEnumerator ProcessReactions()
        {
            var coroutines = handlers.Where(p => p.Value.Count > 0).Select(p => p.Value.HandleActions()).ToList();

            if (coroutines.Count > 0)
            {
                GlobalState.EventService.Dispatch(new Event.StartReactionsEvent());

                coroutines.Add(ProcessReactions());
                this.StartCoroutinesSequential(coroutines);
                yield break;
            }

            GlobalState.EventService.Dispatch(new Event.ReactionsFinishedEvent());
        }
    }
}
