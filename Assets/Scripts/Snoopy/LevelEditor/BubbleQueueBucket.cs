using UnityEngine;
using UnityEngine.UI;
using LevelEditor;
using Snoopy.Model;
using System;

namespace Snoopy.LevelEditor
{
    public class BubbleQueueBucket : BubbleWeightEditor
    {
        public event Action OnBucketChanged;

        [SerializeField]
        private Text label;

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Toggle mandatoryToggle;

        [SerializeField]
        private Button insertButton;

        [SerializeField]
        private Button deleteButton;

        private BubbleQueueDefinition.Bucket bucket;

        public BubbleQueueDefinition.Bucket Bucket { get { return bucket; } }

        public bool EnableInsert
        {
            get { return insertButton.gameObject.activeSelf; }
            set { insertButton.gameObject.SetActive(value); }
        }

        public bool EnableDelete
        {
            get { return deleteButton.gameObject.activeSelf; }
            set { deleteButton.gameObject.SetActive(value); }
        }

        public string Label
        {
            get { return label.text; }
            set { label.text = value; }
        }

        public bool ShowMandatoryOption
        {
            get { return mandatoryToggle.gameObject.activeSelf; }
            set { mandatoryToggle.gameObject.SetActive(value); }
        }

        public void Start()
        {
            inputField.onEndEdit.AddListener(OnEndLengthEdit);
            mandatoryToggle.onValueChanged.AddListener(OnMandatoryValueChanged);
        }

        public void Initialize(BubbleFactory factory, BubbleQueuePanel panel, BubbleQueueDefinition.Bucket bucket)
        {
            this.bucket = bucket;

            CreateWeightElements(factory, bucket.counts);

            inputField.text = bucket.length.ToString();
            mandatoryToggle.isOn = bucket.mandatory;

            insertButton.onClick.AddListener(() => panel.InsertBucket(this));
            deleteButton.onClick.AddListener(() => panel.RemoveBucket(this));
        }

        private void OnEndLengthEdit(string text)
        {
            bucket.length = Mathf.Max(1, int.Parse(text));

            if (OnBucketChanged != null)
            {
                OnBucketChanged();
            }
        }

        private void OnMandatoryValueChanged(bool value)
        {
            bucket.mandatory = value;
        }
    }
}
