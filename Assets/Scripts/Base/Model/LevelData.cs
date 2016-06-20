using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using BubbleContent;
using Goal;

namespace Model
{
    [Serializable]
    public sealed class LevelData
    {
        [Serializable]
        public sealed class BubbleData
        {
            public BubbleType Type { get { return (BubbleType)type; } }
            public BubbleContentType ContentType { get { return (BubbleContentType)content_type; } }
            public int X { get { return x; } }
            public int Y { get { return y; } }

            [SerializeField]
            private int type;

            [SerializeField]
            private int content_type;

            [SerializeField]
            private int x;

            [SerializeField]
            private int y;

            [NonSerialized]
            public Bubble model;
        }

        public int ShotCount { get { return shot_count; } }
        public float[] PowerUpFills { get { return power_up_fills; } }
        public IEnumerable<BubbleData> Bubbles { get { return bubbles; } }

        [XmlIgnore]
        public List<LevelGoal> goals;

        [SerializeField]
        private int shot_count;

        [SerializeField]
        private float[] power_up_fills;

        [SerializeField]
        private BubbleData[] bubbles;
    }
}
