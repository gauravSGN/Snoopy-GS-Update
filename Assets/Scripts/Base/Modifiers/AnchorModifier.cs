using Model;
using UnityEngine;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.Anchor)]
    public class AnchorModifier : EditorBubbleModifier
    {
        override public string SpriteName { get { return "Anchor Modifier"; } }
        override public BubbleModifierType ModifierType { get { return BubbleModifierType.Anchor; } }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
            var model = target.GetComponent<BubbleModelBehaviour>().Model;

            model.IsRoot = true;

            var overlayPrefab = model.definition.AnchorOverlayPrefab;

            if (overlayPrefab != null)
            {
                var overlay = GameObject.Instantiate(overlayPrefab, target.transform) as GameObject;
                overlay.transform.parent.GetComponent<BubbleDeath>().DeactivateObjectOnDeath(overlay);
            }
        }
    }
}