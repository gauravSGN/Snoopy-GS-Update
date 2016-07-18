using UnityEngine;
using System.Collections.Generic;
using Util;
using Model;
using LevelEditor.Manipulator;
using LevelEditor.Properties;
using Snoopy.Model;

namespace LevelEditor
{
    public class LevelManipulator : MonoBehaviour
    {
        public class ManipulatorActionFactory : AttributeDrivenFactory<ManipulatorAction, ManipulatorActionAttribute, ManipulatorActionType>
        {
            override protected ManipulatorActionType GetKeyFromAttribute(ManipulatorActionAttribute attribute)
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

        private readonly Dictionary<int, BubbleData> models = new Dictionary<int, BubbleData>();
        private readonly Dictionary<int, GameObject> views = new Dictionary<int, GameObject>();
        private readonly ManipulatorActionFactory actionFactory = new ManipulatorActionFactory();
        private readonly List<string> undoBuffer = new List<string>();
        private readonly List<string> redoBuffer = new List<string>();

        private ManipulatorActionType actionType;
        private ManipulatorAction action;

        private readonly LevelProperties levelProperties = new LevelProperties();
        private readonly BubbleQueueDefinition queue = new BubbleQueueDefinition();
        private List<RandomBubbleDefinition> randoms = new List<RandomBubbleDefinition>();

        public BubbleType BubbleType { get; private set; }

        public Dictionary<int, BubbleData> Models { get { return models; } }
        public Dictionary<int, GameObject> Views { get { return views; } }

        public GameObject BubblePrefab { get { return bubblePrefab; } }
        public RectTransform BubbleContainer { get { return bubbleContainer; } }

        public BubbleFactory BubbleFactory { get { return bubbleFactory; } }
        public ManipulatorActionFactory ActionFactory { get { return actionFactory; } }

        public LevelProperties LevelProperties { get { return levelProperties; } }
        public BubbleQueueDefinition Queue { get { return queue; } }
        public List<RandomBubbleDefinition> Randoms { get { return randoms; } }

        public void LoadLevel(string jsonText)
        {
            var levelData = JsonUtility.FromJson<LevelData>(jsonText);
            var placer = new PlaceBubbleAction();

            foreach (var bubble in levelData.Bubbles)
            {
                BubbleType = bubble.Type;

                placer.Perform(this, bubble.X, bubble.Y);
            }

            LevelProperties.FromLevelData(levelData);
            LevelProperties.NotifyListeners();

            queue.CopyFrom(levelData.Queue);
            queue.ShotCount = levelData.ShotCount;
            queue.NotifyListeners();

            randoms = new List<RandomBubbleDefinition>(levelData.Randoms ?? new RandomBubbleDefinition[0]);

            GlobalState.Instance.Services.Get<Service.EventService>().Dispatch(new LevelEditorLoadEvent());
        }

        public string SaveLevel()
        {
            var data = new MutableLevelData
            {
                Bubbles = models.Values,
                Queue = queue,
                Randoms = randoms.ToArray(),
                ShotCount = queue.ShotCount,
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
            if (BubbleType != type)
            {
                BubbleType = type;
                Debug.Log(string.Format("LevelManipulator: Bubble type is now {0}", type));
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

        public void Undo()
        {
            if (undoBuffer.Count > 0)
            {
                var state = undoBuffer[undoBuffer.Count - 1];
                undoBuffer.RemoveAt(undoBuffer.Count - 1);

                RestoreState(state);
            }
        }

        public void Redo()
        {
            if (redoBuffer.Count > 0)
            {
                var state = redoBuffer[redoBuffer.Count - 1];
                redoBuffer.RemoveAt(redoBuffer.Count - 1);

                RestoreState(state);
            }
        }

        public void BeginNewAction()
        {
            undoBuffer.Add(SaveLevel());
            redoBuffer.Clear();

            while (undoBuffer.Count > LevelEditorConstants.UNDO_BUFFER_SIZE)
            {
                undoBuffer.RemoveAt(0);
            }
        }

        private void RestoreState(string state)
        {
            var saveBubbleType = BubbleType;
            var saveActionType = actionType;

            var clear = new ClearAction();
            clear.Perform(this, 0, 0);

            LoadLevel(state);

            SetBubbleType(saveBubbleType);
            SetActionType(saveActionType);
        }
    }
}
