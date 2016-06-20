using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using BubbleContent;
using Goal;

namespace Model
{
    [Serializable]
    public class LevelData
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

            public BubbleData() { }

            public BubbleData(int x, int y, BubbleType type)
            {
                this.x = x;
                this.y = y;
                this.type = (int)type;
            }
        }

        public virtual int ShotCount { get { return shot_count; } }
        public virtual float[] PowerUpFills { get { return power_up_fills; } }
        public virtual IEnumerable<BubbleData> Bubbles { get { return bubbles; } }

        [XmlIgnore]
        public List<LevelGoal> goals;

        [SerializeField]
        protected int shot_count;

        [SerializeField]
        protected float[] power_up_fills;

        [SerializeField]
        protected BubbleData[] bubbles;
    }
}
