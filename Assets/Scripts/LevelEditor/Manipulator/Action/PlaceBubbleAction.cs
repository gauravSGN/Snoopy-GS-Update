using Model;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.PlaceBubble)]
    public class PlaceBubbleAction : ManipulatorAction
    {
        private readonly DeleteBubbleAction deleter = new DeleteBubbleAction();

        public Sprite ButtonSprite
        {
            get { return Resources.Load("Textures/UI/PlaceBubbleButton", typeof(Sprite)) as Sprite; }
        }

        public static Vector3 GetBubbleLocation(int x, int y)
        {
            var offset = (y & 1) * LevelEditorConstants.HALF_SIZE;

            return new Vector3(
                LevelEditorConstants.HALF_SIZE + x * LevelEditorConstants.BUBBLE_SIZE + offset + 2,
                -(LevelEditorConstants.HALF_SIZE + y * LevelEditorConstants.ROW_HEIGHT + 2)
            );
        }

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            PlaceBubble(manipulator, x, y, manipulator.BubbleType);
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
                deleter.Perform(manipulator, x, y);
        }

        private void PlaceBubble(LevelManipulator manipulator, int x, int y, BubbleType type)
        {
            deleter.Perform(manipulator, x, y);

            var definition = manipulator.BubbleFactory.GetDefinitionByType(type);
            var prefabRenderer = definition.Prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabRenderer != null)
            {
                var instance = GameObject.Instantiate(manipulator.BubblePrefab);

                var image = instance.GetComponent<Image>();
                image.sprite = prefabRenderer.sprite;

                instance.name = string.Format("{0} ({1}, {2})", type, x, y);
                instance.transform.SetParent(manipulator.BubbleContainer, false);
                instance.transform.localPosition = GetBubbleLocation(x, y);

                var model = new BubbleData(x, y, type);
                manipulator.Views.Add(model.Key, instance);
                manipulator.Models.Add(model.Key, model);
            }
        }
    }
}
