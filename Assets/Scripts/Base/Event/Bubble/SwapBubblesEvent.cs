sealed public class SwapBubblesEvent : PooledEvent<SwapBubblesEvent>
{
    static public void Dispatch()
    {
        DispatchPooled();
    }
}
