namespace Aiming
{
    sealed public class StartAimingEvent : PooledEvent<StartAimingEvent>
    {
        public static void Dispatch()
        {
            DispatchPooled();
        }
    }
}
