using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class GenericPopup : Popup
    {
        [SerializeField]
        private Text bannerText;

        [SerializeField]
        private Text mainText;

        override public void Setup(PopupConfig genericConfig)
        {
            base.Setup(genericConfig);
            var config = genericConfig as GenericPopupConfig;

            bannerText.text = config.title;
            mainText.text = config.mainText;
        }
    }
}