using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Snoopy.Model;
using LevelEditor;

namespace Snoopy.LevelEditor
{
    public class BubbleQueuePanel : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private RectTransform contentContainer;

        [SerializeField]
        private InputField moveCountField;

        [SerializeField]
        private GameObject bucketPrefab;

        [SerializeField]
        private float rowHeight;

        private readonly List<BubbleQueueBucket> buckets = new List<BubbleQueueBucket>();
        private BubbleQueueDefinition queue;

        public void Start()
        {
            queue = manipulator.Queue;
            queue.AddListener(OnQueueChanged);

            moveCountField.onEndEdit.AddListener(OnMoveCountEndEdit);

            Initialize();
        }

        public void AddBucket()
        {
            InsertBucketAtIndex(queue.buckets.Count);
        }

        public void InsertBucket(BubbleQueueBucket bucket)
        {
            InsertBucketAtIndex(buckets.IndexOf(bucket));
        }

        public void RemoveBucket(BubbleQueueBucket bucket)
        {
            var index = buckets.IndexOf(bucket);

            if (index >= 0)
            {
                RemoveBucketAtIndex(index);
                queue.buckets.RemoveAt(index);
                OnBucketChanged();
            }
        }

        private void Initialize()
        {
            foreach (var bucket in queue.buckets)
            {
                buckets.Add(CreateBucket(bucket, true));
            }

            buckets.Add(CreateBucket(queue.reserve, false));
            buckets.Add(CreateBucket(queue.extras, false));

            moveCountField.text = queue.ShotCount.ToString();

            OnBucketChanged();
        }

        private void OnQueueChanged(Observable target)
        {
            while (buckets.Count > 0)
            {
                RemoveBucketAtIndex(0);
            }

            Initialize();
        }

        private void InsertBucketAtIndex(int index)
        {
            queue.buckets.Insert(index, new BubbleQueueDefinition.Bucket());
            buckets.Insert(index, CreateBucket(queue.buckets[index], true));

            OnBucketChanged();
        }

        private void RemoveBucketAtIndex(int index)
        {
            Destroy(buckets[index].gameObject);
            buckets.RemoveAt(index);
        }

        private BubbleQueueBucket CreateBucket(BubbleQueueDefinition.Bucket bucket, bool canBeMandatory)
        {
            var instance = Instantiate(bucketPrefab);
            var component = instance.GetComponent<BubbleQueueBucket>();

            instance.transform.SetParent(contentContainer, false);
            component.Initialize(manipulator.BubbleFactory, this, bucket);
            component.ShowMandatoryOption = canBeMandatory;
            component.EnableInsert = canBeMandatory;
            component.EnableDelete = canBeMandatory;
            component.OnBucketChanged += OnBucketChanged;

            return component;
        }

        private void OnBucketChanged()
        {
            int offset = 0;
            var count = buckets.Count;

            contentContainer.sizeDelta = new Vector2(0.0f, rowHeight * count);

            for (var index = 0; index < count - 2; index++)
            {
                var bucket = buckets[index];
                var length = bucket.Bucket.length;

                if (length == 1)
                {
                    bucket.Label = string.Format("{0}", offset + 1);
                }
                else
                {
                    bucket.Label = string.Format("{0}-{1}", offset + 1, offset + length);
                }

                offset += length;
            }

            for (var index = 0; index < count; index++)
            {
                buckets[index].transform.SetSiblingIndex(index);
            }

            buckets[count - 2].Label = "Reserve";
            buckets[count - 1].Label = "Extras";
        }

        private void OnMoveCountEndEdit(string value)
        {
            try
            {
                queue.ShotCount = Mathf.Max(1, int.Parse(value));
            }
            catch (System.FormatException)
            {
                queue.ShotCount = 1;
                moveCountField.text = queue.ShotCount.ToString();
            }
        }
    }
}
