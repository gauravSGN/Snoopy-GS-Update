using UI.Popup;
using UnityEngine;

namespace UI.Callbacks
{
    public class PurchaseHearts : MonoBehaviour
    {
        public void Start()
        {
            if (GlobalState.User.purchasables.hearts.quantity == 0)
            {
                GlobalState.PopupService.EnqueueWithDelay(1.0f, new StandalonePopupConfig(PopupType.OutOfHearts));
            }
        }

        public void Buy()
        {
            if (GlobalState.User.purchasables.hearts.quantity < GlobalState.Instance.Config.purchasables.maxHearts)
            {
                GlobalState.PopupService.Enqueue(new StandalonePopupConfig(PopupType.OutOfHearts));
            }
        }
    }
}