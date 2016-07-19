using UnityEngine;
using UnityEngine.UI;
using Model;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.Random)]
    public class RandomBubbleModifier : BubbleModifier
    {
        private Sprite sprite;

        override public BubbleModifierType ModifierType { get { return BubbleModifierType.Random; } }

        public RandomBubbleModifier()
        {
            sprite = Resources.Load("Textures/Modifiers/RandomBubble", typeof(Sprite)) as Sprite;
        }

        override protected void ModifyBubbleData(BubbleData bubbleData, BubbleData.ModifierData data)
        {
            // This modifier makes no changes to the bubble data.
        }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
        }

        override protected void ModifyEditorObject(GameObject target, BubbleData.ModifierData data)
        {
            target.GetComponent<Image>().sprite = sprite;
            AddTextToBubble(target, string.Format("R{0}", int.Parse(data.data) + 1));
        }
    }
}
