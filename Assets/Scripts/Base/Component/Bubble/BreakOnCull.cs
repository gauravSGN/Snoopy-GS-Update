public class BreakOnCull : BubbleModelBehaviour
{
    override protected void AddListeners()
    {
        Model.OnDisconnected += OnDisconnected;
    }

    override protected void RemoveListeners()
    {
        Model.OnDisconnected -= OnDisconnected;
    }

    private void OnDisconnected(Bubble bubble)
    {
        RemoveListeners();
        gameObject.layer = (int)Layers.FallingObjects;

        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Cull);
    }
}
