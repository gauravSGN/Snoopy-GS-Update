using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

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

        public BubbleType Type { get { return (BubbleType)type; } }
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

        public BubbleData(int x, int y, BubbleType type)
        {
            this.x = x;
            this.y = y;
            this.type = (int)type;
        }

        public BubbleData CloneAt(int x, int y)
        {
            return new BubbleData(x, y, Type)
            {
                modifiers = modifiers.Select(m => new ModifierData { type = m.type, data = m.data }).ToArray(),
            };
        }
    }
}
