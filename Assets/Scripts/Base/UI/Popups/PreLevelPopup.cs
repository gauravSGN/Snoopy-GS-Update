using FTUE;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI.Popup
{
    public class PreLevelPopup : Popup
    {
        [SerializeField]
        private Text bannerText;

        [SerializeField]
        private Sprite filledStar;

        [SerializeField]
        private GameObject[] starPositions;

        private PreLevelPopupConfig config;

        override public void Setup(PopupConfig genericConfig)
        {
            base.Setup(genericConfig);
            config = genericConfig as PreLevelPopupConfig;

            bannerText.text = "Level " + config.level.ToString();

            for (long starIndex = 0, filledStars = config.stars; starIndex < filledStars; ++starIndex)
            {
                starPositions[starIndex].GetComponent<Image>().sprite = filledStar;
            }

            config.closeActions = new List<Action> { () => GlobalState.SceneService.Reset() };
            config.affirmativeActions = new List<Action> { TransitionToLevel };

            GlobalState.EventService.Dispatch(new TutorialProgressEvent(TutorialTrigger.PreLevelPopup, config.level));
        }

        private void TransitionToLevel()
        {
            GlobalState.User.currentLevel = config.level;
            GlobalState.SceneService.OnFinishedLoading += DispatchToTurnOffInput;
            GlobalState.SceneService.TransitionToScene(config.nextScene);
        }

        private void DispatchToTurnOffInput()
        {
            GlobalState.SceneService.OnFinishedLoading -= DispatchToTurnOffInput;
            GlobalState.EventService.Dispatch(new InputToggleEvent(false));
        }
    }
}