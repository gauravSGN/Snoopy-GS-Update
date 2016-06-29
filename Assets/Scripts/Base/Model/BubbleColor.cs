using System;

[Flags]
public enum BubbleColor
{
    None = 0,

    Blue = 1 << 0,
    Red = 1 << 1,
    Yellow = 1 << 2,
    Green = 1 << 3,
    Purple = 1 << 4,
    Pink = 1 << 5,

    Rainbow = Blue | Red | Yellow | Green | Purple | Pink,
}
