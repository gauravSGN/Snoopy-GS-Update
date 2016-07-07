using UnityEngine;
using System.Collections.Generic;
using System;

namespace Snoopy.Model
{
    [Serializable]
    public sealed class BubbleQueueDefinition
    {
        [Serializable]
        public sealed class Bucket
        {
            public bool mandatory;
            public int length;
            public int[] counts = new int[6];
        }

        public List<Bucket> buckets = new List<Bucket>();
        public Bucket reserve = new Bucket();
        public Bucket extras = new Bucket();
    }
}
