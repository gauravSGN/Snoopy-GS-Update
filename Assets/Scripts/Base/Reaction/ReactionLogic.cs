using Util;
using System;
using UI.Popup;
using System.Linq;
using UnityEngine;
using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
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

            if (GetComponent<Level>().levelState.remainingBubbles <= 0)
            {
                GlobalState.EventService.Dispatch(new LevelCompleteEvent(false));


                GlobalState.User.purchasables.hearts.quantity--;

                GlobalState.PopupService.Enqueue(new GenericPopupConfig
                {
                    title = "Level Lost",
                    mainText = "Hearts Left: " + GlobalState.User.purchasables.hearts.quantity.ToString(),
                    closeActions = new List<Action> { DispatchReturnToMap },
                    affirmativeActions = new List<Action> { DispatchReturnToMap }
                });
            }
            else
            {
                GlobalState.EventService.Dispatch(new ReadyForNextBubbleEvent());
            }
        }

        private void DispatchReturnToMap()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }
    }
}
