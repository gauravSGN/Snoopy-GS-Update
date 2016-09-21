using PowerUps;

namespace FTUE
{
    sealed public class PowerUpUsedEvent : GameEvent
    {
        public PowerUpType type;

        public PowerUpUsedEvent(PowerUpType type)
        {
            this.type = type;
        }
    }
}
