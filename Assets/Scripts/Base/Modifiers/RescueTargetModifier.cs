using UnityEngine;
using UnityEngine.UI;
using Model;
using Service;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.RescueTarget)]
    public class RescueTargetModifier : BubbleModifier
    {
        private const string SPRITE_PATH = "Textures/Modifiers/woodstock";

        private Sprite sprite;

        override public BubbleModifierType ModifierType { get { return BubbleModifierType.RescueTarget; } }

        override protected void ModifyBubbleData(BubbleData bubbleData, BubbleData.ModifierData data)
        {
            // This modifier makes no changes to the bubble data.
        }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
            sprite = sprite ?? GlobalState.Instance.Services.Get<AssetService>().LoadAsset<Sprite>(SPRITE_PATH);

            var rescueSprite = CreateRescueSprite(target);

            rescueSprite.AddComponent<SpriteRenderer>().sprite = sprite;
        }

        override protected void ModifyEditorObject(GameObject target, BubbleData.ModifierData data)
        {
            sprite = sprite ?? GlobalState.Instance.Services.Get<AssetService>().LoadAsset<Sprite>(SPRITE_PATH);

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
