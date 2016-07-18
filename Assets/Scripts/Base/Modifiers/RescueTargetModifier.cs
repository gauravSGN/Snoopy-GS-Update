using UnityEngine;
using Model;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.RescueTarget)]
    public class RescueTargetModifier : BubbleModifier
    {
        private Sprite sprite;

        override public BubbleModifierType ModifierType { get { return BubbleModifierType.RescueTarget; } }

        public RescueTargetModifier()
        {
            sprite = Resources.Load("Textures/Modifiers/BabyPanda", typeof(Sprite)) as Sprite;
        }

        override protected void ModifyBubbleData(BubbleData bubbleData, BubbleData.ModifierData data)
        {
            // This modifier makes no changes to the bubble data.
        }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
            var thingInside = new GameObject();
            thingInside.name = "RescueTarget";

            thingInside.transform.SetParent(target.transform, false);
            thingInside.transform.localPosition = Vector3.back;

            var renderer = thingInside.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }
    }
}
