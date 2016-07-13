public class WeightedBubbleQueueTests : BubbleQueueTests
{
    override protected BubbleQueue GetBubbleQueue(LevelState levelstate)
    {
        return new WeightedBubbleQueue(levelstate);
    }
}