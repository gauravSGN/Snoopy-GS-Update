namespace Aiming
{
    sealed public class StopAimingEvent : PooledEvent<StopAimingEvent>
    {
        public static void Dispatch()
        {
            DispatchPooled();
        }
    }
}
