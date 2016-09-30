using Model;
using UnityEngine;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.Anchor)]
    public class AnchorModifier : EditorBubbleModifier
    {
        override public BubbleModifierType ModifierType { get { return BubbleModifierType.Anchor; } }
        override public string SpriteName { get { return "Anchor Modifier"; } }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
            target.GetComponent<BubbleModelBehaviour>().Model.IsRoot = true;
        }
    }
}