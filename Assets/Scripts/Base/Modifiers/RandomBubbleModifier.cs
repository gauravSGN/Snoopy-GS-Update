using UnityEngine;
using UnityEngine.UI;
using Model;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.Random)]
    public class RandomBubbleModifier : BubbleModifier
    {
        private const string SPRITE_PATH = "Textures/Modifiers/RandomBubble";

        private Sprite sprite;

        override public BubbleModifierType ModifierType { get { return BubbleModifierType.Random; } }

        override protected void ModifyBubbleData(BubbleData bubbleData, BubbleData.ModifierData data)
        {
            bubbleData.Type = Configuration.Randoms[int.Parse(data.data)].GetValue();
        }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
            // This modifier makes no changes to the game object.
        }

        override protected void ModifyEditorObject(GameObject target, BubbleData.ModifierData data)
        {
            sprite = sprite ?? GlobalState.AssetService.LoadAsset<Sprite>(SPRITE_PATH);

            target.GetComponent<Image>().sprite = sprite;
            AddTextToBubble(target, string.Format("R{0}", int.Parse(data.data) + 1));
        }
    }
}
