using UnityEngine;

public class AimPositionEvent : GameEvent
{
    public Vector3 position;

    public static void Dispatch(Vector3 position)
    {
        var gameEvent = GlobalState.EventService.GetPooledEvent<AimPositionEvent>();

        gameEvent.position = position;

        GlobalState.EventService.DispatchPooled(gameEvent);
    }
}
