using Util;
using System;
using Service;
using UI.Popup;
using System.Linq;
using UnityEngine;
using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;

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
            var eventService = GlobalState.Instance.Services.Get<EventService>();

            eventService.AddEventHandler<BubbleSettledEvent>(OnBubbleSettled);
            eventService.AddEventHandler<BubbleReactionEvent>(OnBubbleReactionEvent);

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
            var coroutines = handlers.Where(p => p.Value.Count > 0).Select(p => p.Value.HandleActions()).ToList();

            if (coroutines.Count > 0)
            {
                coroutines.Add(ProcessReactions());
                this.StartCoroutinesSequential(coroutines);
                yield break;
            }

            if (GetComponent<Level>().levelState.remainingBubbles <= 0)
            {
                GlobalState.Instance.Services.Get<UserStateService>().purchasables.hearts.quantity--;

                GlobalState.Instance.Services.Get<PopupService>().Enqueue(new GenericPopupConfig
                {
                    title = "Level Lost",
                    mainText = "Hearts Left: " + GlobalState.Instance.Services.Get<UserStateService>().purchasables.hearts.quantity.ToString(),
                    closeActions = new List<Action> { DispatchLevelLost },
                    affirmativeActions = new List<Action> { DispatchLevelLost }
                });
            }
            else
            {
                GlobalState.Instance.Services.Get<EventService>().Dispatch(new ReadyForNextBubbleEvent());
            }
        }

        private void DispatchLevelLost()
        {
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new LevelCompleteEvent(false));
        }
    }
}
