using PowerUps;

namespace FTUE
{
    sealed public class PowerUpFilledEvent : GameEvent
    {
        public PowerUpType type;

        public PowerUpFilledEvent(PowerUpType type)
        {
            this.type = type;
        }
    }
}
