using UnityEngine;
using System.Collections.Generic;
using System;

namespace Model
{
    [Serializable]
    public sealed class RandomBubbleDefinition
    {
        public List<int> exclusions = new List<int>();
        public int[] weights = new int[6];
        public ChainedRandomizer<BubbleType>.SelectionMethod rollType = ChainedRandomizer<BubbleType>.SelectionMethod.Once;
    }
}
