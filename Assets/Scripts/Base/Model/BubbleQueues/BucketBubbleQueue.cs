public class BucketBubbleQueue : BaseBubbleQueue, BubbleQueue
{
    // get and hold level bucket data
    // track current bucket
    // fill bucket when it is empty
    // drop bucket if past move count
    // drop to reserve bucket if bubble eliminated
    // drop to extra moves bucket if past end of level
    abstract protected BubbleType GenerateElement();
    abstract protected void BuildQueue();
    abstract protected void RemoveInactiveTypes();
}