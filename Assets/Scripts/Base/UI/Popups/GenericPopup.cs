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

        private bool clickedOk;
        private GenericPopupConfig config;

        public void Okay()
        {
            clickedOk = true;
            Close();
        }

        override public void Setup(PopupConfig genericConfig)
        {
            config = genericConfig as GenericPopupConfig;

            bannerText.text = config.title;
            mainText.text = config.mainText;
        }

        override protected void OnCloseTweenComplete(AbstractGoTween tween)
        {
            base.OnCloseTweenComplete(tween);

            var actionsToProcess = (clickedOk ? config.affirmativeActions : config.closeActions);

            foreach (var action in actionsToProcess)
            {
                action.Invoke();
            }
        }
    }
}