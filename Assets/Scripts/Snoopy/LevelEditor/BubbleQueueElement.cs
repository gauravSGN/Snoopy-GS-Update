using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Snoopy.Model;

namespace Snoopy.LevelEditor
{
    public class BubbleQueueElement : MonoBehaviour
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private Color disabledColor;

        private InputField inputField;
        private BubbleQueueDefinition.Bucket bucket;
        private Color textColor;
        private int index;

        public void Start()
        {
            inputField = GetComponent<InputField>();
            inputField.onEndEdit.AddListener(OnEndEdit);
            UpdateText();
        }

        public void Initialize(BubbleQueueDefinition.Bucket bucket, int index, Color color)
        {
            this.bucket = bucket;
            this.index = index;
            textColor = color;

            UpdateText();
        }

        private void OnEndEdit(string text)
        {
            try
            {
                bucket.counts[index] = int.Parse(text);
            }
            catch (System.FormatException)
            {
                bucket.counts[index] = 0;
            }

            UpdateText();
        }

        private void UpdateText()
        {
            if (inputField != null)
            {
                var value = bucket.counts[index];
                inputField.text = (value <= 0) ? "-" : value.ToString();
                text.color = (value <= 0) ? disabledColor : textColor;
            }
        }
    }
}
