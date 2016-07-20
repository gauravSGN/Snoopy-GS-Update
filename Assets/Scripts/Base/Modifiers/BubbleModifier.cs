using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Modifiers
{
    abstract public class BubbleModifier
    {
        public abstract BubbleModifierType ModifierType { get; }
        public LevelConfiguration Configuration { get; set; }

        abstract protected void ModifyBubbleData(BubbleData bubbleData, BubbleData.ModifierData data);
        abstract protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data);
        abstract protected void ModifyEditorObject(GameObject target, BubbleData.ModifierData data);

        public void ApplyDataModifications(BubbleData data)
        {
            Apply(data.modifiers, m => ModifyBubbleData(data, m));
        }

        public void ApplyGameObjectModifications(BubbleData data, GameObject target)
        {
            Apply(data.modifiers, m => ModifyGameObject(target, m));
        }

        public void ApplyEditorModifications(BubbleData data, GameObject target)
        {
            Apply(data.modifiers, m => ModifyEditorObject(target, m));
        }

        protected void AddTextToBubble(GameObject target, string text)
        {
            var textComponent = target.GetComponentInChildren<Text>();
            var values = textComponent.text.Split(' ').ToList();
            values.Add(text);
            values.Sort();
            textComponent.text = string.Join(" ", values.ToArray());
        }

        private void Apply(IEnumerable<BubbleData.ModifierData> data, Action<BubbleData.ModifierData> action)
        {
            foreach (var modifier in data)
            {
                if (modifier.type == ModifierType)
                {
                    action(modifier);
                }
            }
        }
    }
}
