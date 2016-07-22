using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Model;
using LevelEditor.Manipulator;
using Util;
using Service;

namespace LevelEditor
{
    sealed public class RandomStateSaver : SingletonBehaviour<RandomStateSaver>
    {
        private delegate BubbleData BubbleTransformation(BubbleData data);

        private List<BubbleData> savedState;

        void Start()
        {
            if (Instance == this)
            {
                GlobalState.Instance.Services.Get<EventService>().AddEventHandler<ClearLevelEvent>(OnClearLevel);
            }
        }

        public void Reset(LevelManipulator manipulator)
        {
            if (savedState != null)
            {
                ApplyTransformation(manipulator, b => b);
                savedState = null;
            }
        }

        public void Apply(LevelManipulator manipulator)
        {
            Reset(manipulator);

            SaveCurrentState(manipulator);
            RollAllRandomBubbles(manipulator);
        }

        private void SaveCurrentState(LevelManipulator manipulator)
        {
            savedState = manipulator.Models.Values
                .Where(b => (b.modifiers != null) && (b.modifiers.Any(m => m.type == BubbleModifierType.Random)))
                .ToList();
        }

        private void RollAllRandomBubbles(LevelManipulator manipulator)
        {
            var rng = new System.Random();
            var randoms = LevelConfiguration.CreateRandomizers(rng, manipulator.Randoms.ToArray());

            ApplyTransformation(manipulator, b =>
            {
                var modifier = b.modifiers.First(m => m.type == BubbleModifierType.Random);
                var replacement = new BubbleData(b.X, b.Y, randoms[int.Parse(modifier.data)].GetValue());

                var modifiers = b.modifiers.Where(m => m.type != BubbleModifierType.Random).ToArray();
                if (modifiers.Length > 0)
                {
                    replacement.modifiers = modifiers;
                }

                return replacement;
            });
        }

        private void ApplyTransformation(LevelManipulator manipulator, BubbleTransformation transformation)
        {
            var placer = new PlaceBubbleAction();

            manipulator.PushState();

            foreach (var bubble in savedState)
            {
                if (manipulator.Models.ContainsKey(bubble.Key))
                {
                    var replacement = transformation(bubble);

                    manipulator.Models[replacement.Key] = replacement;
                    manipulator.SetBubbleType(replacement.Type);
                    placer.Perform(manipulator, replacement.X, replacement.Y);
                }
            }

            manipulator.PopState();
        }

        private void OnClearLevel(ClearLevelEvent gameEvent)
        {
            savedState = null;
        }
    }
}
