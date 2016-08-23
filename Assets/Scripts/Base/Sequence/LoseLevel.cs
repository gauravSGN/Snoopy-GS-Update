using System;
using UI.Popup;
using System.Collections.Generic;

namespace Sequence
{
    public class LoseLevel : BaseSequence<LevelState>
    {
        override public void Begin(LevelState parameters)
        {
            StartCoroutine(RunActionAfterDelay(2.0f, () =>
            {
                GlobalState.EventService.Dispatch(new LevelCompleteEvent(false));

                GlobalState.User.purchasables.hearts.quantity--;

                GlobalState.PopupService.EnqueueWithDelay(1.0f, new GenericPopupConfig
                {
                    title = "Level Lost",
                    mainText = "Hearts Left: " + GlobalState.User.purchasables.hearts.quantity.ToString(),
                    closeActions = new List<Action> { TransitionToReturnScene },
                    affirmativeActions = new List<Action> { TransitionToReturnScene }
                });
            }));
        }
    }
}