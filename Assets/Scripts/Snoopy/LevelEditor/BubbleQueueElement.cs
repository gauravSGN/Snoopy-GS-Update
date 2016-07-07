using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Snoopy.Model;

namespace Snoopy.LevelEditor
{
    public class BubbleQueueElement : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Image image;

        private BubbleQueueDefinition.Bucket bucket;
        private int index;

        public Sprite Sprite
        {
            get { return image.sprite; }
            set { image.sprite = value; }
        }

        public void Start()
        {
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        public void Initialize(BubbleQueueDefinition.Bucket bucket, int index)
        {
            this.bucket = bucket;
            this.index = index;

            inputField.text = bucket.counts[index].ToString();
        }

        private void OnEndEdit(string text)
        {
            bucket.counts[index] = int.Parse(text);
        }
    }
}
