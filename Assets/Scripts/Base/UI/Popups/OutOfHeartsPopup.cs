using System;
using System.Collections.Generic;

namespace UI.Popup
{
    public class OutOfHeartsPopup : Popup
    {
        override public void Setup(PopupConfig genericConfig)
        {
            base.Setup(genericConfig);
            genericConfig.affirmativeActions = new List<Action> { BuyHeartsRefill };
        }

        private void BuyHeartsRefill()
        {
            var purchasables = GlobalState.User.purchasables;

            if (purchasables.coins.quantity < 40)
            {
                // TODO: Show store flow here?
            }

            purchasables.coins.quantity -= 40;
            purchasables.hearts.quantity += GlobalState.Instance.Config.purchasables.maxHearts;
        }
    }
}