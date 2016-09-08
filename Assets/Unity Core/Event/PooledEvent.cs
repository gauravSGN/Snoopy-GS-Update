using System;

abstract public class PooledEvent<T> : GameEvent where T : PooledEvent<T>
{
    static protected void DispatchPooled()
    {
        GlobalState.EventService.DispatchPooled(GlobalState.EventService.GetPooledEvent<T>());
    }

    static protected void DispatchPooled(Action<T> initializer)
    {
        var gameEvent = GlobalState.EventService.GetPooledEvent<T>();

        initializer(gameEvent);

        GlobalState.EventService.DispatchPooled(gameEvent);
    }
}
