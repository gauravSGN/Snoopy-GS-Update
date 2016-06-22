using System;

namespace PowerUps
{
    [Flags]
    public enum PowerUpType
    {
        Yellow = 1,
        Pink = 2,
        Blue = 4,
        Red = 8,
        Purple = 16,
        Green = 32,
    }
}
