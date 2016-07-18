using System;

namespace PowerUps
{
    [Flags]
    public enum PowerUpType
    {
        Blue = 1,
        Yellow = 2,
        Red = 4,
        Green = 8,
        Pink = 16,
        Purple = 32,
    }
}
