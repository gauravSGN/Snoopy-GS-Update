using Service;

public class CoinsTextUpdater : UITextUpdater
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
