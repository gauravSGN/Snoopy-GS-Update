using State;
using Service;
using UnityEngine;

public class CoinsTextUpdater : TextUpdater
{
    override protected void Start()
    {
        base.Start();

        var user = GlobalState.Instance.Services.Get<UserStateService>();

        if (user != null)
        {
            Target = user.purchasables.coins;
        }
    }
}
