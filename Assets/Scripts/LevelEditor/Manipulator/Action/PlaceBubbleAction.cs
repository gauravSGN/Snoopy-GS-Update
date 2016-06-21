using Model;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.PlaceBubble)]
    public class PlaceBubbleAction : ManipulatorAction
    {
        private const int BUBBLE_SIZE = 32;
        private const float HALF_SIZE = BUBBLE_SIZE / 2.0f;

        private readonly DeleteBubbleAction deleter = new DeleteBubbleAction();

        public Sprite ButtonSprite
        {
            get { return Resources.Load("Textures/UI/PlaceBubbleButton", typeof(Sprite)) as Sprite; }
        }

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            deleter.Perform(manipulator, x, y);

            var definition = manipulator.BubbleFactory.GetBubbleDefinitionByType(manipulator.BubbleType);
            var prefabRenderer = definition.prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabRenderer != null)
            {
                var instance = GameObject.Instantiate(manipulator.BubblePrefab);

                var image = instance.GetComponent<Image>();
                image.sprite = prefabRenderer.sprite;

                instance.name = string.Format("{0} ({1}, {2})", manipulator.BubbleType, x, y);
                instance.transform.SetParent(manipulator.BubbleContainer, false);
                instance.transform.localPosition = GetBubbleLocation(x, y);

                var key = y << 4 | x;
                manipulator.Views.Add(key, instance);
                manipulator.Models.Add(key, new LevelData.BubbleData(x, y, manipulator.BubbleType));
            }
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            deleter.Perform(manipulator, x, y);
        }

        private Vector3 GetBubbleLocation(int x, int y)
        {
            var offset = (y & 1) * BUBBLE_SIZE / 2.0f;

            return new Vector3(
                HALF_SIZE + x * BUBBLE_SIZE + offset + 2,
                -(HALF_SIZE + y * BUBBLE_SIZE * MathUtil.COS_30_DEGREES + 2)
            );
        }
    }
}
