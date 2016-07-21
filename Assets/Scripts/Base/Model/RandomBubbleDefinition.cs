using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Model
{
    [Serializable]
    public sealed class RandomBubbleDefinition
    {
        public List<int> exclusions = new List<int>();
        public int[] weights = new int[6];
        public ChainedRandomizer<BubbleType>.SelectionMethod rollType = ChainedRandomizer<BubbleType>.SelectionMethod.Once;

        public RandomBubbleDefinition Clone()
        {
            return new RandomBubbleDefinition
            {
                exclusions = exclusions.ToList(),
                weights = weights.ToArray(),
                rollType = rollType,
            };
        }

        public bool Compare(RandomBubbleDefinition other)
        {
            return (other != null) &&
                   (rollType == other.rollType) &&
                   weights.SequenceEqual(other.weights) &&
                   exclusions.SequenceEqual(other.exclusions);
        }
    }
}
