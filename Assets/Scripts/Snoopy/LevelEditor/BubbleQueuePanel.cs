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
        private Text totalText;

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

            Initialize();
        }

        public void AddBucket()
        {
            var index = queue.buckets.Count;

            queue.buckets.Add(new BubbleQueueDefinition.Bucket());
            buckets.Insert(index, CreateBucket(queue.buckets[index], true));

            OnBucketChanged();
        }

        public void RemoveBucket()
        {
            var index = queue.buckets.Count - 1;

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
            component.Initialize(manipulator.BubbleFactory, bucket);
            component.ShowMandatoryOption = canBeMandatory;
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
                    bucket.Label = string.Format("Bubble {0}", offset + 1);
                }
                else
                {
                    bucket.Label = string.Format("Bubbles {0}-{1}", offset + 1, offset + length);
                }

                offset += length;
            }

            for (var index = 0; index < count; index++)
            {
                buckets[index].transform.SetSiblingIndex(index);
            }

            buckets[count - 2].Label = "Reserve";
            buckets[count - 1].Label = "Extras";
            totalText.text = string.Format("Total = {0}", offset);
        }
    }
}
