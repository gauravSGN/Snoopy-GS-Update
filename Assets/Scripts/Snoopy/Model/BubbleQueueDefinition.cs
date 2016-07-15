using System.Collections.Generic;
using System;

namespace Snoopy.Model
{
    [Serializable]
    public sealed class BubbleQueueDefinition : Observable
    {
        [Serializable]
        public sealed class Bucket
        {
            public Bucket() : this(0, 0, 0, 0, 0, 0) { }

            public Bucket(params int[] values)
            {
                counts = values;
            }

            public bool mandatory;
            public int length = 1;
            public int[] counts;
        }

        public List<Bucket> buckets = new List<Bucket>();
        public Bucket reserve = new Bucket(1, 1, 1, 1, 1, 1);
        public Bucket extras = new Bucket();

        public int ShotCount { get; set; }

        public void CopyFrom(BubbleQueueDefinition queue)
        {
            if (queue != null)
            {
                buckets = queue.buckets;
                reserve = queue.reserve;
                extras = queue.extras;
            }
        }
    }
}
