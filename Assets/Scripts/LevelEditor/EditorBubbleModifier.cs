using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.BossStartPosition)]
    public class EditorBubbleModifier : BubbleModifier
    {
        private const string LIST_PATH = "LevelEditor/BubbleModifierList";

        private BubbleModifierList modifierList;

        override public BubbleModifierType ModifierType { get { return BubbleModifierType.BossStartPosition; } }
        virtual public string SpriteName { get { return "Editor Bubble Modifier"; } }

        override protected void ModifyBubbleData(BubbleData bubbleData, BubbleData.ModifierData data)
        {
        }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
        }

        override protected void ModifyEditorObject(GameObject target, BubbleData.ModifierData data)
        {
            modifierList = modifierList ?? GlobalState.AssetService.LoadAsset<BubbleModifierList>(LIST_PATH);

            var definition = modifierList.GetDefinitionFromTypeAndData(ModifierType, data.data);
            var sprite = EditorBubbleModifier.CreateSprite(target, SpriteName);
            var image = sprite.AddComponent<Image>();

            image.sprite = definition.Sprite;

            var rectTransform = sprite.GetComponent<RectTransform>();
            rectTransform.sizeDelta = target.GetComponent<RectTransform>().sizeDelta;

        }

        // Is there a better home for this method?
        protected static GameObject CreateSprite(GameObject parent, string name)
        {
            var sprite = new GameObject();
            sprite.name = name;

            sprite.transform.SetParent(parent.transform, false);
            sprite.transform.localPosition = Vector3.back;

            return sprite;
        }
    }
}
