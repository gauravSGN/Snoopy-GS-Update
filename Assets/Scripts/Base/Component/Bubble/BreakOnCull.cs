public class BreakOnCull : BubbleCuller
{
    override protected void CullBubble(Bubble bubble)
    {
        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Cull);
    }
}
