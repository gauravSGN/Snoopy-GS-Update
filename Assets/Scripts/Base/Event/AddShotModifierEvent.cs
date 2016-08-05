public class AddShotModifierEvent : GameEvent
{
    public ShotModifierType type;

    public AddShotModifierEvent(ShotModifierType type)
    {
        this.type = type;
    }
}
