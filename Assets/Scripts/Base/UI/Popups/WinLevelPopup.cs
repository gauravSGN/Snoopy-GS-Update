using FTUE;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class WinLevelPopup : Popup
    {
        [SerializeField]
        private Sprite filledStar;

        [SerializeField]
        private GameObject[] starPositions;

        [SerializeField]
        private Text score;

        [SerializeField]
        private Text topScore;

        override public void Setup(PopupConfig genericConfig)
        {
            base.Setup(genericConfig);
            var config = genericConfig as WinLevelPopupConfig;

            for (long starIndex = 0, filledStars = config.stars; starIndex < filledStars; ++starIndex)
            {
                starPositions[starIndex].GetComponent<Image>().sprite = filledStar;
            }

            score.text = "Score: " + config.score.ToString("N0");
            topScore.text = "Top Score: " + config.topScore.ToString("N0");

            GlobalState.EventService.Dispatch(new TutorialProgressEvent(TutorialTrigger.PostLevelPopup, config.level));
        }
    }
}