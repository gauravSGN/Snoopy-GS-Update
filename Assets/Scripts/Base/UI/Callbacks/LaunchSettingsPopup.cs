using gs;
using System;
using Service;
using UI.Popup;
using UnityEngine;
using System.Collections.Generic;

namespace UI.Callbacks
{
    public class LaunchSettingsPopup : MonoBehaviour
    {
        [SerializeField]
        private Level level;

        public void DisplayPause()
        {
            if (level != null)
            {
                GlobalState.PopupService.Enqueue(new SettingsPopupConfig
                {
                    title = "Pause",
                    affirmativeActions = new List<Action> { LaunchLevelLossConfirmationPopup },
                });
            }
        }

        public void DisplaySettings()
        {
            GlobalState.PopupService.Enqueue(new SettingsPopupConfig
            {
                title = "Settings",
                userID = "ID: " + GS.Api.ClientId,
                appVersion = "v" + Application.version,
            });
        }

        private void LaunchLevelLossConfirmationPopup()
        {
            if (level.levelState.remainingBubbles != level.levelState.initialShotCount)
            {
                GlobalState.PopupService.Enqueue(new GenericPopupConfig
                {
                    title = "Exit?",
                    mainText = "You will lose a life if you exit!",
                    affirmativeActions = new List<Action>
                    {
                        () =>
                        {
                            GlobalState.User.purchasables.hearts.quantity--;
                            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
                        }
                    }
                });
            }
            else
            {
                GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
            }
        }
    }
}