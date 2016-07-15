using Util;
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

            user.purchasables.coins.quantity += numberOfCoins;
            user.hasPaid = DateTimeUtil.GetUnixTime();
        }
    }
}