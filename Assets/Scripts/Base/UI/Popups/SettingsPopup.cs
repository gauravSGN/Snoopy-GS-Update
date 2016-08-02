using gs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        private bool returnToMap;
        private SettingsPopupConfig config;

        public void ReturnToMap()
        {
            returnToMap = true;
            Close();
        }

        override public void Setup(PopupConfig genericConfig)
        {
            config = genericConfig as SettingsPopupConfig;

            bannerText.text = config.title;
            appVersionText.text = "v" + Application.version;

            if (GS.Api.ClientId != null)
            {
                userIDText.text = "ID: " + GS.Api.ClientId;
            }

            musicToggle.isOn = GlobalState.User.settings.musicOn;
            musicToggle.onValueChanged.AddListener(value => GlobalState.User.settings.musicOn = value);

            sfxToggle.isOn = GlobalState.User.settings.sfxOn;
            sfxToggle.onValueChanged.AddListener(value => GlobalState.User.settings.sfxOn = value);
        }

        override protected void OnCloseTweenComplete(AbstractGoTween tween)
        {
            base.OnCloseTweenComplete(tween);

            if (returnToMap && (SceneManager.GetActiveScene().name != StringConstants.Scenes.MAP))
            {
                SceneManager.LoadScene(StringConstants.Scenes.MAP);
            }
        }
    }
}