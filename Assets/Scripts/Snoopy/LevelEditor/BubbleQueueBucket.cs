using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Snoopy.Model;
using System.Linq;
using System;

namespace Snoopy.LevelEditor
{
    public class BubbleQueueBucket : MonoBehaviour
    {
        public event Action OnBucketChanged;

        [SerializeField]
        private Text label;

        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private RectTransform elementContainer;

        [SerializeField]
        private Toggle mandatoryToggle;

        [SerializeField]
        private Button insertButton;

        [SerializeField]
        private Button deleteButton;

        [SerializeField]
        private GameObject elementPrefab;

        private BubbleQueueDefinition.Bucket bucket;
        private List<BubbleQueueElement> elements = new List<BubbleQueueElement>();

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
            int index = 0;

            foreach (var def in factory.Bubbles.Where(b => b.category == BubbleCategory.Basic))
            {
                var sprite = def.Prefab.GetComponentInChildren<SpriteRenderer>();

                if (sprite != null)
                {
                    var instance = Instantiate(elementPrefab);
                    var element = instance.GetComponent<BubbleQueueElement>();

                    instance.transform.SetParent(elementContainer, false);
                    element.Initialize(bucket, index, def.BaseColor);

                    elements.Add(element);
                    index++;
                }
            }

            inputField.text = bucket.length.ToString();

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
