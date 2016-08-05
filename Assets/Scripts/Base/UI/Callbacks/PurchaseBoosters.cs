using UnityEngine;

namespace UI.Callbacks
{
    public class PurchaseBoosters : MonoBehaviour
    {
        [SerializeField]
        private int numberOfBoosters;

        [SerializeField]
        private int coinCost;

        public void BuyRainbows()
        {
            var purchasables = GlobalState.User.purchasables;

            if ((purchasables.boosters.rainbows == 0) && (purchasables.coins.quantity >= coinCost))
            {
                purchasables.boosters.rainbows += numberOfBoosters;
                purchasables.coins.quantity -= coinCost;
            }
        }
    }
}
