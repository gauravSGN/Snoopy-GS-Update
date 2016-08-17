using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;
using Model;
using LevelEditor.Manipulator;
using LevelEditor.Properties;
using Snoopy.Model;
using Scoring;

namespace LevelEditor
{
    public class LevelManipulator : MonoBehaviour
    {
        public class ManipulatorActionFactory
            : AttributeDrivenFactory<ManipulatorAction, ManipulatorActionAttribute, ManipulatorActionType>
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

        private ManipulatorState state = new ManipulatorState();
        private readonly Stack<ManipulatorState> stateStack = new Stack<ManipulatorState>();

        private readonly LevelProperties levelProperties = new LevelProperties();
        private readonly BubbleQueueDefinition queue = new BubbleQueueDefinition();
        private List<RandomBubbleDefinition> randoms = new List<RandomBubbleDefinition>();
        private string background = LevelEditorConstants.DEFAULT_BACKGROUND;
        private IEnumerator scoreCoroutine;

        public Dictionary<int, BubbleData> Models { get { return models; } }
        public Dictionary<int, GameObject> Views { get { return views; } }

        public GameObject BubblePrefab { get { return bubblePrefab; } }
        public RectTransform BubbleContainer { get { return bubbleContainer; } }

        public BubbleFactory BubbleFactory { get { return bubbleFactory; } }
        public ManipulatorActionFactory ActionFactory { get { return actionFactory; } }

        public LevelProperties LevelProperties { get { return levelProperties; } }
        public BubbleQueueDefinition Queue { get { return queue; } }
        public List<RandomBubbleDefinition> Randoms { get { return randoms; } }
        public string Background
        {
            get { return background; }
            set { background = value; }
        }

        public BubbleType BubbleType { get { return state.BubbleType; } }
        public BubbleModifierInfo Modifier { get { return state.Modifier; } }

        public void LoadLevel(string jsonText)
        {
            var levelData = JsonUtility.FromJson<LevelData>(jsonText);
            var placer = new PlaceBubbleAction();

            randoms = new List<RandomBubbleDefinition>(levelData.Randoms ?? new RandomBubbleDefinition[0]);
            GlobalState.EventService.Dispatch(new RandomBubblesChangedEvent());

            foreach (var bubble in levelData.Bubbles)
            {
                state.BubbleType = bubble.Type;

                placer.Perform(this, bubble.X, bubble.Y);
                bubbleFactory.ApplyEditorModifiers(views[BubbleData.GetKey(bubble.X, bubble.Y)], bubble);
                models[BubbleData.GetKey(bubble.X, bubble.Y)].modifiers = bubble.modifiers;
            }

            LevelProperties.FromLevelData(levelData);
            LevelProperties.StarValues = ScoreUtil.ComputeStarsForLevel(levelData, BubbleFactory);
            LevelProperties.NotifyListeners();

            queue.CopyFrom(levelData.Queue);
            queue.ShotCount = levelData.ShotCount;
            queue.NotifyListeners();

            Background = levelData.Background;

            GlobalState.EventService.Dispatch(new LevelModifiedEvent());
            GlobalState.EventService.Dispatch(new LevelEditorLoadEvent());
        }

        public string SaveLevel()
        {
            var data = new MutableLevelData
            {
                Background = Background,
                Bubbles = models.Values,
                Queue = queue,
                Randoms = randoms.ToArray(),
                ShotCount = queue.ShotCount,
            };

            LevelProperties.StarValues = ScoreUtil.ComputeStarsForLevel(data, BubbleFactory);
            LevelProperties.NotifyListeners();

            LevelProperties.ToLevelData(data);

            return JsonUtility.ToJson(data);
        }

        public void SetActionType(ManipulatorActionType type)
        {
            if (state.ActionType != type)
            {
                state.ActionType = type;
                state.Action = ActionFactory.Create(state.ActionType);
            }
        }

        public void SetBubbleType(BubbleType type)
        {
            state.BubbleType = type;
        }

        public void SetModifier(BubbleModifierInfo modifier)
        {
            state.Modifier = modifier;
        }

        public void PerformAction(int x, int y)
        {
            if (state.Action != null)
            {
                state.Action.Perform(this, x, y);
            }
        }

        public void PerformAlternateAction(int x, int y)
        {
            if (state.Action != null)
            {
                state.Action.PerformAlternate(this, x, y);
            }
        }

        public void Undo()
        {
            if (undoBuffer.Count > 0)
            {
                var previousState = undoBuffer[undoBuffer.Count - 1];
                undoBuffer.RemoveAt(undoBuffer.Count - 1);

                RestoreState(previousState);
                GlobalState.EventService.Dispatch(new LevelModifiedEvent());
            }
        }

        public void Redo()
        {
            if (redoBuffer.Count > 0)
            {
                var nextState = redoBuffer[redoBuffer.Count - 1];
                redoBuffer.RemoveAt(redoBuffer.Count - 1);

                RestoreState(nextState);
                GlobalState.EventService.Dispatch(new LevelModifiedEvent());
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

        public void PushState()
        {
            stateStack.Push(state);
            state = state.Clone();
        }

        public void PopState()
        {
            if (stateStack.Count > 0)
            {
                state = stateStack.Pop();
            }
        }

        public void RecomputeScores()
        {
            if (scoreCoroutine == null)
            {
                scoreCoroutine = DoScoreComputation();
                StartCoroutine(scoreCoroutine);
            }
        }

        private void RestoreState(string state)
        {
            PushState();

            var clear = new ClearAction();
            clear.Perform(this, 0, 0);

            LoadLevel(state);

            PopState();
        }

        private IEnumerator DoScoreComputation()
        {
            yield return null;

            var data = new MutableLevelData
            {
                Background = Background,
                Bubbles = models.Values,
                Queue = queue,
                Randoms = randoms.ToArray(),
                ShotCount = queue.ShotCount,
            };

            var newValues = ScoreUtil.ComputeStarsForLevel(data, BubbleFactory);
            for (int index = 0, count = newValues.Length; index < count; index++)
            {
                newValues[index] = (int)(newValues[index] * LevelProperties.StarMultiplier);
            }

            LevelProperties.StarValues = newValues;
            LevelProperties.NotifyListeners();

            scoreCoroutine = null;
        }
    }
}
