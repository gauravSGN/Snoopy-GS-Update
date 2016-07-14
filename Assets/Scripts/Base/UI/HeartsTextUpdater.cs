using State;
using Service;
using UnityEngine;
using System;

public class HeartsTextUpdater : TextUpdater
{
    [SerializeField]
    private string textWhenFull;

    override protected void Start()
    {
        base.Start();

        var user = GlobalState.Instance.Services.Get<UserStateService>();

        if (user != null)
        {
            Target = user.purchasables;
        }
    }

    override protected void UpdateText(Observable target)
    {
        if (target != null)
        {
            var purchasables = (Purchasables)target;

            if (purchasables.hearts == GlobalState.Instance.Config.purchasables.maxHearts)
            {
                text.text = textWhenFull;
            }
            else
            {
                base.UpdateText(target);
            }
        }
    }
}
