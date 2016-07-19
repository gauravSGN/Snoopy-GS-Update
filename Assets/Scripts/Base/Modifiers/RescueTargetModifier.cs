using UnityEngine;
using UnityEngine.UI;
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
            var rescueSprite = CreateRescueSprite(target);

            rescueSprite.AddComponent<SpriteRenderer>().sprite = sprite;
        }

        override protected void ModifyEditorObject(GameObject target, BubbleData.ModifierData data)
        {
            var rescueSprite = CreateRescueSprite(target);

            var image = rescueSprite.AddComponent<Image>();
            image.sprite = sprite;

            var rectTransform = rescueSprite.GetComponent<RectTransform>();
            rectTransform.sizeDelta = target.GetComponent<RectTransform>().sizeDelta;
        }

        private GameObject CreateRescueSprite(GameObject parent)
        {
            var rescueSprite = new GameObject();
            rescueSprite.name = "RescueTarget";

            rescueSprite.transform.SetParent(parent.transform, false);
            rescueSprite.transform.localPosition = Vector3.back;

            return rescueSprite;
        }
    }
}
