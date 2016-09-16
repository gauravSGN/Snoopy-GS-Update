using System;

namespace Registry
{
    [Flags]
    public enum BlockadeType
    {
        All = -1,
        None = 0,
        AllNonReaction = BlockadeType.Popups & BlockadeType.SceneChange & BlockadeType.Input,

        Popups = 1 << 0,
        SceneChange = 1 << 1,
        Input = 1 << 2,
        Reactions = 1 << 3,


    }
}
