using UnityEngine;
using System.Collections.Generic;
using System;

namespace Model
{
    [Serializable]
    public sealed class RandomBubbleDefinition
    {
        public enum RollType
        {
            Once,
            Each,
        }

        public List<int> exclusions;
        public float[] weights;
        public RollType rollType;
    }
}
