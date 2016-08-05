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

        BlueYellow = 3,
        BlueRed = 5,
        BlueGreen = 9,
        BluePink = 17,
        BluePurple = 33,
        YellowRed = 6,
        YellowGreen = 10,
        YellowPink = 18,
        YellowPurple = 34,
        RedGreen = 12,
        RedPink = 20,
        RedPurple = 36,
        GreenPink = 24,
        GreenPurple = 40,
        PinkPurple = 48,

        ThreeCombo = 64,
        FourCombo = 65,
    }
}
