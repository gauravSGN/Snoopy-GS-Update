using Service;
using UI.Popup;
using UnityEngine;

namespace UI.Callbacks
{
    public class LaunchSettingsPopup : MonoBehaviour
    {
        public void Launch()
        {
            GlobalState.Instance.Services.Get<PopupService>().Enqueue(new SettingsPopupConfig { title = "Settings" });
        }
    }
}