namespace Aiming
{
    sealed public class StartAimingEvent : PooledEvent<StartAimingEvent>
    {
        static public void Dispatch()
        {
            DispatchPooled();
        }
    }
}
