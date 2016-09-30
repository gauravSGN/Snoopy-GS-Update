using LevelEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Snoopy.LevelEditor
{
    public class BossModePanelPopulator : MonoBehaviour
    {
        private const int MAX_PUZZLES = 7;

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private Transform puzzleToggleContainer;

        [SerializeField]
        private InputField numberOfPuzzlesInput;

        [SerializeField]
        private GameObject puzzleTogglePrefab;

        [SerializeField]
        private ToggleGroup puzzleToggleGroup;

        private int numberOfPuzzles = 1;
        private readonly Toggle[] puzzleToggles = new Toggle[MAX_PUZZLES];

        protected void OnEnable()
        {
            Util.FrameUtil.AtEndOfFrame(() => {
                ForceUpdatePuzzleToggleContainer();
            });
        }

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<LevelEditorLoadEvent>(OnLevelEditorLoadEvent);

            numberOfPuzzlesInput.onEndEdit.AddListener(UpdatePuzzleToggleContainer);

            for (var i = 0; i < MAX_PUZZLES; i++)
            {
                var newToggle = Instantiate(puzzleTogglePrefab, puzzleToggleContainer) as GameObject;

                puzzleToggles[i] = newToggle.GetComponentInChildren<Toggle>();
                puzzleToggles[i].group = puzzleToggleGroup;
                puzzleToggles[i].onValueChanged.AddListener(OnPuzzleToggle);

                var label = puzzleToggles[i].gameObject.GetComponentInChildren<Text>();
                label.text += (i + 1);

                puzzleToggles[i].gameObject.SetActive(false);
            }

            ForceUpdatePuzzleToggleContainer();
        }

        private void OnLevelEditorLoadEvent(LevelEditorLoadEvent gameEvent)
        {
            ForceUpdatePuzzleToggleContainer();
            EnableFirstPuzzle();
        }

        private void UpdatePuzzleToggleContainer(string valueAsString)
        {
            var value = int.Parse(valueAsString);

            if ((value >= 1) && (value <= MAX_PUZZLES))
            {
                numberOfPuzzles = value;

                for (var i = 0; i < MAX_PUZZLES; i++)
                {
                    var enabled = (i < numberOfPuzzles);

                    if (!enabled && puzzleToggles[i].isOn)
                    {
                        EnableFirstPuzzle();
                    }

                    puzzleToggles[i].gameObject.SetActive(enabled);
                }
            }

            numberOfPuzzlesInput.text = numberOfPuzzles.ToString();

            if (!puzzleToggleGroup.AnyTogglesOn())
            {
                EnableFirstPuzzle();
            }
        }

        private void OnPuzzleToggle(bool value)
        {
            for (var i = 0; value && (i < MAX_PUZZLES); i++)
            {
                if (puzzleToggles[i].isOn)
                {
                    GlobalState.EventService.Dispatch(new LoadPuzzleEvent { puzzleIndex = i });
                    break;
                }
            }
        }

        private void EnableFirstPuzzle()
        {
            puzzleToggles[0].isOn = true;
        }

        private void ForceUpdatePuzzleToggleContainer()
        {
            UpdatePuzzleToggleContainer(manipulator.NumberOfPuzzles.ToString());
        }
    }
}
