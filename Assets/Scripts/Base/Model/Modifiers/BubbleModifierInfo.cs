using UnityEngine;

namespace Model
{
    public interface BubbleModifierInfo
    {
        BubbleModifierType Type { get; }
        string Data { get; }
        Sprite Sprite { get; }
    }
}
