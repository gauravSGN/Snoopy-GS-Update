using UnityEngine;

sealed public class SetShooterBubbleEvent : PooledEvent<SetShooterBubbleEvent>
{
    public GameObject bubble;

    static public void Dispatch(GameObject bubble)
    {
        DispatchPooled(e => { e.bubble = bubble; });
    }
}
