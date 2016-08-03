using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class SettingsPopup : Popup
    {
        [SerializeField]
        private Text bannerText;

        [SerializeField]
        private Text appVersionText;

        [SerializeField]
        private Text userIDText;

        [SerializeField]
        private Toggle musicToggle;

        [SerializeField]
        private Toggle sfxToggle;

        override public void Setup(PopupConfig genericConfig)
        {
            base.Setup(genericConfig);
            var config = genericConfig as SettingsPopupConfig;

            bannerText.text = config.title;
            appVersionText.text = config.appVersion;
            userIDText.text = config.userID;

            musicToggle.isOn = GlobalState.User.settings.musicOn;
            musicToggle.onValueChanged.AddListener(value => GlobalState.User.settings.musicOn = value);

            sfxToggle.isOn = GlobalState.User.settings.sfxOn;
            sfxToggle.onValueChanged.AddListener(value => GlobalState.User.settings.sfxOn = value);
        }
    }
}