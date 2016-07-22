using Service;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        private bool transitionOnClose;
        private PreLevelPopupConfig config;

        public void Transition()
        {
            transitionOnClose = true;
            Close();
        }

        override public void Setup(PopupConfig genericConfig)
        {
            config = genericConfig as PreLevelPopupConfig;

            bannerText.text = "Level " + config.level.ToString();

            for (long starIndex = 0, filledStars = config.stars; starIndex < filledStars; ++starIndex)
            {
                starPositions[starIndex].GetComponent<Image>().sprite = filledStar;
            }
        }

        override protected void OnCloseTweenComplete(AbstractGoTween tween)
        {
            base.OnCloseTweenComplete(tween);

            if (transitionOnClose)
            {
                SceneManager.LoadScene(config.nextScene);
            }
            else
            {
                GlobalState.Instance.Services.Get<SceneService>().Reset();
            }
        }
    }
}