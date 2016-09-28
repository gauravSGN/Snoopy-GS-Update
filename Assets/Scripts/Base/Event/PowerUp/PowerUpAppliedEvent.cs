using PowerUps;

public class PowerUpAppliedEvent : GameEvent
{
    public PowerUpType type;

    public PowerUpAppliedEvent(PowerUpType type)
    {
        this.type = type;
    }
}
