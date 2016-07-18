using UnityEngine;
using System;
using System.Collections.Generic;

namespace Model
{
    sealed public class BubbleModifierDefinition : ScriptableObject
    {
        [SerializeField]
        private BubbleModifierType type;

        [SerializeField]
        private string data;

        [SerializeField]
        private Sprite sprite;

        public BubbleModifierType Type { get { return type; } }
        public string Data { get { return data; } }
        public Sprite Sprite { get { return sprite; } }
    }
}
