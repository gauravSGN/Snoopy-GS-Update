using System;
using System.Collections.Generic;
using UnityEngine;
using BubbleContent;
using Goal;

namespace Model
{
    [Serializable]
    public class LevelData
    {
        [Serializable]
        sealed public class BubbleData
        {
            public int Key { get { return GetKey(X, Y); } }

            public BubbleType Type { get { return (BubbleType)type; } }
            public BubbleContentType ContentType { get { return (BubbleContentType)contentType; } }
            public int X { get { return x; } }
            public int Y { get { return y; } }

            [SerializeField]
            private int type;

            [SerializeField]
            private int contentType;

            [SerializeField]
            private int x;

            [SerializeField]
            private int y;

            [NonSerialized]
            public Bubble model;

            public BubbleData() { }

            public static int GetKey(int x, int y)
            {
                return y << 4 | x;
            }

            public BubbleData(int x, int y, BubbleType type)
            {
                this.x = x;
                this.y = y;
                this.type = (int)type;
            }

            public BubbleData(int x, int y, BubbleType bubbleType, BubbleContentType contentType)
                : this(x, y, bubbleType)
            {
                this.contentType = (int)contentType;
            }
        }

        virtual public int ShotCount { get { return shotCount; } }
        virtual public float[] PowerUpFills { get { return powerUpFills; } }
        virtual public IEnumerable<BubbleData> Bubbles { get { return bubbles; } }

        public List<LevelGoal> goals;

        [SerializeField]
        protected int shotCount;

        [SerializeField]
        protected float[] powerUpFills;

        [SerializeField]
        protected BubbleData[] bubbles;
    }
}
