using Service;
using UnityEngine;

namespace UI.Callbacks
{
    public class PurchaseCoins : MonoBehaviour
    {
        [SerializeField]
        private int numberOfCoins;

        public void Buy()
        {
            GlobalState.Instance.Services.Get<UserStateService>().purchasables.coins.quantity += numberOfCoins;
        }
    }
}