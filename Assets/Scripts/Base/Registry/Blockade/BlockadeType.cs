using System;

namespace Registry
{
    [Flags]
    public enum BlockadeType
    {
        All = -1,
        None = 0,

        Popups = 1 << 0,
        SceneChange = 1 << 1,
    }
}
