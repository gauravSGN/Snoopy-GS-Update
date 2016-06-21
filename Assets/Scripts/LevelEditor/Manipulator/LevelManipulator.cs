using UnityEngine;
using System.Collections.Generic;
using Util;
using Model;
using LevelEditor.Manipulator;
using BubbleContent;
using LevelEditor.Properties;

namespace LevelEditor
{
    public class LevelManipulator : MonoBehaviour
    {
        public class ManipulatorActionFactory : AttributeDrivenFactory<ManipulatorAction, ManipulatorActionAttribute, ManipulatorActionType>
        {
            protected override ManipulatorActionType GetKeyFromAttribute(ManipulatorActionAttribute attribute)
            {
                return attribute.ActionType;
            }
        }

        [SerializeField]
        private RectTransform bubbleContainer;

        [SerializeField]
        private GameObject bubblePrefab;

        [SerializeField]
        private BubbleFactory bubbleFactory;

        private readonly Dictionary<int, LevelData.BubbleData> models = new Dictionary<int, LevelData.BubbleData>();
        private readonly Dictionary<int, GameObject> views = new Dictionary<int, GameObject>();
        private readonly ManipulatorActionFactory actionFactory = new ManipulatorActionFactory();

        private ManipulatorActionType actionType;
        private ManipulatorAction action;

        private readonly LevelProperties levelProperties = new LevelProperties();

        public BubbleType BubbleType { get; private set; }
        public BubbleContentType ContentType { get; private set; }

        public Dictionary<int, LevelData.BubbleData> Models { get { return models; } }
        public Dictionary<int, GameObject> Views { get { return views; } }

        public GameObject BubblePrefab { get { return bubblePrefab; } }
        public RectTransform BubbleContainer { get { return bubbleContainer; } }

        public BubbleFactory BubbleFactory { get { return bubbleFactory; } }
        public ManipulatorActionFactory ActionFactory { get { return actionFactory; } }

        public LevelProperties LevelProperties { get { return levelProperties; } }

        public void LoadLevel(string jsonText)
        {
            var levelData = JsonUtility.FromJson<LevelData>(jsonText);
            var placer = new PlaceBubbleAction();

            foreach (var bubble in levelData.Bubbles)
            {
                BubbleType = bubble.Type;
                ContentType = BubbleContentType.None;

                placer.Perform(this, bubble.X, bubble.Y);

                if (bubble.ContentType != BubbleContentType.None)
                {
                    ContentType = bubble.ContentType;
                    placer.Perform(this, bubble.X, bubble.Y);
                }
            }

            LevelProperties.FromLevelData(levelData);
            LevelProperties.NotifyListeners();
        }

        public string SaveLevel()
        {
            var data = new MutableLevelData
            {
                Bubbles = models.Values,
            };

            LevelProperties.ToLevelData(data);

            return JsonUtility.ToJson(data);
        }

        public void SetActionType(ManipulatorActionType type)
        {
            if (actionType != type)
            {
                actionType = type;
                action = ActionFactory.Create(actionType);
                Debug.Log(string.Format("LevelManipulator: Action type is now {0}", type));
            }
        }

        public void SetBubbleType(BubbleType type)
        {
            if ((BubbleType != type) || (ContentType != BubbleContentType.None))
            {
                BubbleType = type;
                ContentType = BubbleContentType.None;
                Debug.Log(string.Format("LevelManipulator: Bubble type is now {0}", type));
            }
        }

        public void SetContentType(BubbleContentType type)
        {
            if (ContentType != type)
            {
                ContentType = type;
                Debug.Log(string.Format("LevelManipulator: Content type is now {0}", type));
            }
        }

        public void PerformAction(int x, int y)
        {
            if (action != null)
            {
                action.Perform(this, x, y);
            }
        }

        public void PerformAlternateAction(int x, int y)
        {
            if (action != null)
            {
                action.PerformAlternate(this, x, y);
            }
        }
    }
}
