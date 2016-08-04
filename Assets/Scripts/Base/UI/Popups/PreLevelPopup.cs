using System;
using Service;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
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

            config.closeActions = new List<Action> { () => GlobalState.Instance.Services.Get<SceneService>().Reset() };
            config.affirmativeActions = new List<Action> { () => SceneManager.LoadScene(config.nextScene) };
        }
    }
}