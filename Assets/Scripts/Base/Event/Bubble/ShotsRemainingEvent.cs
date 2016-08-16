public class ShotsRemainingEvent : GameEvent
{
    public int shots;

    public ShotsRemainingEvent(int shots)
    {
        this.shots = shots;
    }
}
