using UnityEngine;
using UnityEngine.UI;

namespace Snoopy.LevelEditor
{
    public class BossModePanelPopulator : MonoBehaviour
    {
        private const int MAX_PUZZLES = 7;

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

        protected void Start()
        {
            numberOfPuzzlesInput.onEndEdit.AddListener(UpdatePuzzleToggleContainer);

            for (var i = 0; i < MAX_PUZZLES; i++)
            {
                var newToggle = Instantiate(puzzleTogglePrefab, puzzleToggleContainer) as GameObject;

                puzzleToggles[i] = newToggle.GetComponentInChildren<Toggle>();
                puzzleToggles[i].group = puzzleToggleGroup;
                puzzleToggles[i].onValueChanged.AddListener(OnPuzzleToggle);

                var label = puzzleToggles[i].gameObject.GetComponentInChildren<Text>();
                label.text += (i + 1);
            }

            UpdatePuzzleToggleContainer("1");
            puzzleToggles[0].isOn = true;
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
                    puzzleToggles[i].gameObject.SetActive(enabled);

                    // This puzzle is no longer valid, so load the first puzzle which must exist
                    if (!enabled && puzzleToggles[i].isOn)
                    {
                        puzzleToggles[0].isOn = true;
                    }
                }
            }
            else
            {
                numberOfPuzzlesInput.text = numberOfPuzzles.ToString();
            }
        }

        private void OnPuzzleToggle(bool value)
        {
            if (value)
            {
                for (var i = 0; i < MAX_PUZZLES; i++)
                {
                    if (puzzleToggles[i].isOn)
                    {
                        Debug.Log("load puzzle " + (i + 1));
                    }
                }
            }
        }
    }
}
