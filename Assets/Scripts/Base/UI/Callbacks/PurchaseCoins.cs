using Util;
using UnityEngine;

namespace UI.Callbacks
{
    public class PurchaseCoins : MonoBehaviour
    {
        [SerializeField]
        private int numberOfCoins;

        public void Buy()
        {
            GlobalState.User.purchasables.coins.quantity += numberOfCoins;
            GlobalState.User.hasPaid = DateTimeUtil.GetUnixTime();
        }
    }
}