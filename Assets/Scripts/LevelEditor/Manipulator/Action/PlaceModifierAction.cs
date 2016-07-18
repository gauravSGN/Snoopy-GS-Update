using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.PlaceModifier)]
    public class PlaceModifierAction : ManipulatorAction
    {
        public Sprite ButtonSprite { get { return null; } }

        private PlaceBubbleAction placer = new PlaceBubbleAction();

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            if (manipulator.Models.ContainsKey(BubbleData.GetKey(x, y)) && (manipulator.Modifier != null))
            {
                RemoveModifier(manipulator, x, y, manipulator.Modifier);
                ApplyModifier(manipulator, x, y, manipulator.Modifier);
                placer.Perform(manipulator, x, y);
            }
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            if (manipulator.Models.ContainsKey(BubbleData.GetKey(x, y)) && (manipulator.Modifier != null))
            {
                RemoveModifier(manipulator, x, y, manipulator.Modifier);
                placer.Perform(manipulator, x, y);
            }
        }

        private void ApplyModifier(LevelManipulator manipulator, int x, int y, BubbleModifierDefinition modifier)
        {
            var model = manipulator.Models[BubbleData.GetKey(x, y)];
            var modifiers = new List<BubbleData.ModifierData>(model.modifiers ?? new BubbleData.ModifierData[0]);

            modifiers.Add(new BubbleData.ModifierData
            {
                type = modifier.Type,
                data = modifier.Data,
            });

            model.modifiers = modifiers.ToArray();
        }

        private void RemoveModifier(LevelManipulator manipulator, int x, int y, BubbleModifierDefinition modifier)
        {
            var model = manipulator.Models[BubbleData.GetKey(x, y)];

            if (model.modifiers != null)
            {
                model.modifiers = model.modifiers.Where(m => m.type != modifier.Type).ToArray();

                if (model.modifiers.Length == 0)
                {
                    model.modifiers = null;
                }
            }
        }
    }
}
