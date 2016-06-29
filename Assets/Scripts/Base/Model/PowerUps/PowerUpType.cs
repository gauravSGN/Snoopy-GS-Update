using System;

namespace PowerUps
{
    [Flags]
    public enum PowerUpType
    {
        Yellow = 1,
        Green = 2,
        Blue = 4,
        Red = 8,
        Purple = 16,
        Pink = 32,
    }
}
