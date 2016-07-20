using UnityEngine;
using System.Collections.Generic;

namespace Model
{
    sealed public class BubbleModifierData : BubbleModifierInfo
    {
        public BubbleModifierType Type { get; set; }
        public string Data { get; set; }
        public Sprite Sprite { get; set; }
    }
}
