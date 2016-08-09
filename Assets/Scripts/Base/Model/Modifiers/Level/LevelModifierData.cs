using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    sealed public class LevelModifierData
    {
        [SerializeField]
        private LevelModifierType type;

        [SerializeField]
        private string data;

        public LevelModifierType Type { get { return type; } }
        public string Data { get { return data; } }

        public LevelModifierData() : this(LevelModifierType.None, null) {}

        public LevelModifierData(LevelModifierType type, string data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
