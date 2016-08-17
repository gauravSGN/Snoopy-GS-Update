public class CoinsTextUpdater : UITextUpdater
{
    override protected void Start()
    {
        base.Start();

        if (GlobalState.User != null)
        {
            Target = GlobalState.User.purchasables.coins;
        }
    }
}
