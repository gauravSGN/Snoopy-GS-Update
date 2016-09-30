using Model;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.BossStartPosition)]
    public class BossStartPositionModifier : EditorBubbleModifier
    {
        override public BubbleModifierType ModifierType { get { return BubbleModifierType.BossStartPosition; } }
        override public string SpriteName { get { return "Boss Start Position Modifier"; } }
    }
}