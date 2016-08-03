using System;
using Service;
using UI.Popup;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
                GlobalState.Instance.Services.Get<PopupService>().Enqueue(new SettingsPopupConfig
                {
                    title = "Pause",
                    affirmativeActions = new List<Action> { LaunchLevelLossConfirmationPopup },
                });
            }
        }

        public void DisplaySettings()
        {
            GlobalState.Instance.Services.Get<PopupService>().Enqueue(new SettingsPopupConfig { title = "Settings" });
        }

        private void LaunchLevelLossConfirmationPopup()
        {
            if (level.levelState.remainingBubbles != level.levelState.initialShotCount)
            {
                GlobalState.Instance.Services.Get<PopupService>().Enqueue(new GenericPopupConfig
                {
                    title = "Exit?",
                    mainText = "You will lose a life if you exit!",
                    affirmativeActions = new List<Action>
                    {
                        () =>
                        {
                            GlobalState.User.purchasables.hearts.quantity--;
                            SceneManager.LoadScene(StringConstants.Scenes.MAP);
                        }
                    }
                });
            }
            else
            {
                SceneManager.LoadScene(StringConstants.Scenes.MAP);
            }
        }
    }
}