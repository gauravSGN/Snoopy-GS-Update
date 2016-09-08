namespace Aiming
{
    sealed public class StopAimingEvent : PooledEvent<StopAimingEvent>
    {
        static public void Dispatch()
        {
            DispatchPooled();
        }
    }
}
