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

                var model = new LevelData.BubbleData(x, y, type);
                manipulator.Views.Add(model.Key, instance);
                manipulator.Models.Add(model.Key, model);
            }
        }

        private void ReplaceContents(LevelManipulator manipulator, int x, int y, BubbleContentType type)
        {
            var key = LevelData.BubbleData.GetKey(x, y);

            if (manipulator.Models.ContainsKey(key))
            {
                var definition = manipulator.ContentFactory.GetDefinitionByType(type);
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
            var key = LevelData.BubbleData.GetKey(x, y);

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
