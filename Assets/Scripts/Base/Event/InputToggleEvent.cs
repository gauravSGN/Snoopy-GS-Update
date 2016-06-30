public class InputToggleEvent : GameEvent
{
    public bool enabled;

    public InputToggleEvent(bool enabled)
    {
        this.enabled = enabled;
    }
}
