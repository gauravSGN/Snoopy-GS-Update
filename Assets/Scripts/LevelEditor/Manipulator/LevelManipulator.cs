using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Util;
using Model;
using LevelEditor.Manipulator;

namespace LevelEditor
{
    public class LevelManipulator : MonoBehaviour
    {
        private class ManipulatorActionFactory : AttributeDrivenFactory<ManipulatorAction, ManipulatorActionAttribute, ManipulatorActionType>
        {
            protected override ManipulatorActionType GetKeyFromAttribute(ManipulatorActionAttribute attribute)
            {
                return attribute.ActionType;
            }
        }

        private const int BUBBLE_SIZE = 32;
        private const float HALF_SIZE = BUBBLE_SIZE / 2.0f;

        [SerializeField]
        private RectTransform bubbleContainer;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private BubbleFactory factory;

        private LevelData levelData;
        private readonly Dictionary<int, LevelData.BubbleData> models = new Dictionary<int, LevelData.BubbleData>();
        private readonly Dictionary<int, GameObject> views = new Dictionary<int, GameObject>();

        private ManipulatorActionFactory actionFactory = new ManipulatorActionFactory();
        private ManipulatorActionType actionType;
        private ManipulatorAction action;
        private BubbleType bubbleType;

        public void Clear()
        {
            models.Clear();
            views.Clear();

            for (var index = bubbleContainer.childCount - 1; index >= 0; index--)
            {
                Destroy(bubbleContainer.GetChild(index).gameObject);
            }
        }

        public void LoadLevel(string jsonText)
        {
            levelData = JsonUtility.FromJson<LevelData>(jsonText);

            foreach (var bubble in levelData.Bubbles)
            {
                PlaceBubble(bubble.X, bubble.Y, bubble.Type);
            }
        }

        public void PlaceBubble(int x, int y, BubbleType type)
        {
            var definition = factory.GetBubbleDefinitionByType(type);
            var prefabRenderer = definition.prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabRenderer != null)
            {
                var instance = Instantiate(prefab);

                var image = instance.GetComponent<Image>();
                image.sprite = prefabRenderer.sprite;

                instance.transform.SetParent(bubbleContainer, false);
                instance.transform.localPosition = GetBubbleLocation(x, y);
            }
        }

        public void SetActionType(ManipulatorActionType type)
        {
            if (actionType != type)
            {
                actionType = type;
                action = actionFactory.Create(actionType);
                Debug.Log(string.Format("LevelManipulator: Action type is now {0}", type));
            }
        }

        public void SetBubbleType(BubbleType type)
        {
            if (bubbleType != type)
            {
                bubbleType = type;
                Debug.Log(string.Format("LevelManipulator: Bubble type is now {0}", type));
            }
        }

        public void PerformAction(int x, int y)
        {
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
