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
            var user = GlobalState.Instance.Services.Get<UserStateService>();

            if (user != null)
            {
                user.purchasables.coins.quantity += numberOfCoins;
            }
        }
    }
}