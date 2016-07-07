using UnityEngine;
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
        private GameObject bucketPrefab;

        [SerializeField]
        private float rowHeight;

        private readonly List<BubbleQueueBucket> buckets = new List<BubbleQueueBucket>();
        private BubbleQueueDefinition queue = new BubbleQueueDefinition();

        public void Start()
        {
            buckets.Add(CreateBucket(queue.reserve));
            buckets.Add(CreateBucket(queue.extras));

            contentContainer.sizeDelta = new Vector2(0.0f, rowHeight * 2.0f);
        }

        private BubbleQueueBucket CreateBucket(BubbleQueueDefinition.Bucket bucket)
        {
            var instance = Instantiate(bucketPrefab);
            var component = instance.GetComponent<BubbleQueueBucket>();

            instance.transform.SetParent(contentContainer, false);
            component.Initialize(manipulator.BubbleFactory, bucket);

            return component;
        }
    }
}
