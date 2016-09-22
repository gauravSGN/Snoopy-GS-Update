using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class PuzzleData
    {
        virtual public IEnumerable<BubbleData> Bubbles { get { return bubbles; } }

        [SerializeField]
        protected BubbleData[] bubbles;
    }
}
