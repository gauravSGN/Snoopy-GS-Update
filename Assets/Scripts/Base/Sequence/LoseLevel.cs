using System;
using UI.Popup;
using System.Collections.Generic;

namespace Sequence
{
    public class LoseLevel : BaseSequence<LevelState>
    {
        override public void Begin(LevelState parameters)
        {
            StartCoroutine(RunActionAfterDelay(GlobalState.Instance.Config.level.levelLostDelay, () =>
            {
                GlobalState.EventService.Dispatch(new LevelCompleteEvent(false));

                GlobalState.User.purchasables.hearts.quantity--;

                GlobalState.PopupService.Enqueue(new StandalonePopupConfig(PopupType.OutOfMoves)
                {
                    closeActions = new List<Action> { EndLevel },
                    affirmativeActions = new List<Action> { BuyMoreMoves },
                });
            }));
        }

        private void EndLevel()
        {
            GlobalState.PopupService.Enqueue(new GenericPopupConfig
            {
                title = "Level Lost",
                mainText = "Hearts Left: " + GlobalState.User.purchasables.hearts.quantity.ToString(),
                closeActions = new List<Action> { TransitionToReturnScene },
                affirmativeActions = new List<Action> { TransitionToReturnScene },
            });
       }

        private void BuyMoreMoves()
        {
            var coins = GlobalState.User.purchasables.coins;

            if (coins.quantity < 40)
            {
                // TODO: Show store flow here?
            }

            coins.quantity = Math.Max(0, coins.quantity - 40);
            GlobalState.EventService.Dispatch(new PurchasedExtraMovesEvent());
        }
    }
}
