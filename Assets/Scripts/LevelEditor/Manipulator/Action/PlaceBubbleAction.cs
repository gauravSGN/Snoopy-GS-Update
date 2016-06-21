using BubbleContent;
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
            if (manipulator.ContentType == BubbleContentType.None)
            {
                PlaceBubble(manipulator, x, y, manipulator.BubbleType);
            }
            else
            {
                ReplaceContents(manipulator, x, y, manipulator.ContentType);
            }
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            if (manipulator.ContentType == BubbleContentType.None)
            {
                deleter.Perform(manipulator, x, y);
            }
            else
            {
                RemoveContents(manipulator, x, y);
            }
        }

        private Vector3 GetBubbleLocation(int x, int y)
        {
            var offset = (y & 1) * BUBBLE_SIZE / 2.0f;

            return new Vector3(
                HALF_SIZE + x * BUBBLE_SIZE + offset + 2,
                -(HALF_SIZE + y * BUBBLE_SIZE * MathUtil.COS_30_DEGREES + 2)
            );
        }

        private void PlaceBubble(LevelManipulator manipulator, int x, int y, BubbleType type)
        {
            deleter.Perform(manipulator, x, y);

            var definition = manipulator.BubbleFactory.GetBubbleDefinitionByType(type);
            var prefabRenderer = definition.prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabRenderer != null)
            {
                var instance = GameObject.Instantiate(manipulator.BubblePrefab);

                var image = instance.GetComponent<Image>();
                image.sprite = prefabRenderer.sprite;

                instance.name = string.Format("{0} ({1}, {2})", type, x, y);
                instance.transform.SetParent(manipulator.BubbleContainer, false);
                instance.transform.localPosition = GetBubbleLocation(x, y);

                var key = y << 4 | x;
                manipulator.Views.Add(key, instance);
                manipulator.Models.Add(key, new LevelData.BubbleData(x, y, type));
            }
        }

        private void ReplaceContents(LevelManipulator manipulator, int x, int y, BubbleContentType type)
        {
            var key = y << 4 | x;

            if (manipulator.Models.ContainsKey(key))
            {
                var definition = manipulator.BubbleFactory.GetContentDefinitionByType(type);
                var prefabRenderer = definition.prefab.GetComponentInChildren<SpriteRenderer>();
                var model = manipulator.Models[key];
                var view = manipulator.Views[key];

                if (prefabRenderer != null)
                {
                    RemoveAllChildren(view.transform);
                    var instance = GameObject.Instantiate(manipulator.BubblePrefab);

                    var image = instance.GetComponent<Image>();
                    image.sprite = prefabRenderer.sprite;

                    instance.name = string.Format("{0}", type);
                    instance.transform.SetParent(view.transform, false);
                    instance.transform.localPosition = Vector3.zero;

                    manipulator.Models.Remove(key);
                    manipulator.Models.Add(key, new LevelData.BubbleData(x, y, model.Type, type));
                }
            }
        }

        private void RemoveContents(LevelManipulator manipulator, int x, int y)
        {
            var key = y << 4 | x;

            if (manipulator.Models.ContainsKey(key))
            {
                var model = manipulator.Models[key];

                RemoveAllChildren(manipulator.Views[key].transform);

                manipulator.Models.Remove(key);
                manipulator.Models.Add(key, new LevelData.BubbleData(x, y, model.Type));
            }
        }

        private void RemoveAllChildren(Transform parent)
        {
            for (var index = parent.childCount - 1; index >= 0; index--)
            {
                GameObject.Destroy(parent.GetChild(index).gameObject);
            }
        }
    }
}
