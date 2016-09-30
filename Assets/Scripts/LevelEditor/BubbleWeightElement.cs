using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class BubbleWeightElement : MonoBehaviour
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private Color disabledColor;

        private InputField inputField;
        private Color textColor;
        private int[] values;
        private int index;

        public void Start()
        {
            inputField = GetComponent<InputField>();
            inputField.onEndEdit.AddListener(OnEndEdit);
            UpdateText();
        }

        public void Initialize(int[] values, int index, Color color)
        {
            this.values = values;
            this.index = index;
            textColor = color;

            UpdateText();
        }

        private void OnEndEdit(string text)
        {
            try
            {
                values[index] = int.Parse(text);
            }
            catch (System.FormatException)
            {
                values[index] = 0;
            }

            UpdateText();
        }

        private void UpdateText()
        {
            if (inputField != null)
            {
                var value = values[index];
                inputField.text = (value <= 0) ? "-" : value.ToString();
                text.color = (value <= 0) ? disabledColor : textColor;
            }
        }
    }
}
