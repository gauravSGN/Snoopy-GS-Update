sealed public class PlayerIdleEvent : GameEvent
{
    public bool idle;

    public PlayerIdleEvent(bool idle)
    {
        this.idle = idle;
    }
}
