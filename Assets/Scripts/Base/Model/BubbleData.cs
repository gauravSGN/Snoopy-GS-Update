using Util;
using System;
using System.Linq;
using UnityEngine;

namespace Model
{
    [Serializable]
    sealed public class BubbleData
    {
        [Serializable]
        sealed public class ModifierData
        {
            [SerializeField]
            public BubbleModifierType type;

            [SerializeField]
            public string data;
        }

        public int Key { get { return GetKey(X, Y); } }
        public Vector3 WorldPosition { get { return GetWorldPosition(X, Y); } }

        public BubbleType Type
        {
            get { return (BubbleType)type; }
            set { type = (int)value; }
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }

        [SerializeField]
        private int type;

        [SerializeField]
        private int x;

        [SerializeField]
        private int y;

        [SerializeField]
        public ModifierData[] modifiers;

        [NonSerialized]
        public Bubble model;

        public BubbleData() { }

        public static int GetKey(int x, int y)
        {
            return y << 4 | x;
        }

        public static Vector3 GetWorldPosition(int x, int y)
        {
            var rowDistance = GlobalState.Instance.Config.bubbles.size * MathUtil.COS_30_DEGREES;
            var topEdge = Camera.main.orthographicSize - (0.5f * rowDistance);
            var config = GlobalState.Instance.Config;
            var offset = (y & 1) * config.bubbles.size / 2.0f;
            var leftEdge = -(config.bubbles.numPerRow - 1) * config.bubbles.size / 2.0f;

            return new Vector3(leftEdge + x * config.bubbles.size + offset, topEdge - y * rowDistance);
        }

        public BubbleData(int x, int y, BubbleType type)
        {
            this.x = x;
            this.y = y;
            this.type = (int)type;
        }

        public BubbleData CloneAt(int x, int y)
        {
            var result = new BubbleData(x, y, Type);

            if (modifiers != null)
            {
                result.modifiers = modifiers.Select(m => new ModifierData { type = m.type, data = m.data }).ToArray();
            }

            return result;
        }
    }
}
