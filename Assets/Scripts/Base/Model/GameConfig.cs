using UnityEngine;
using System;
using System.Collections;

public class GameConfig : ScriptableObject
{
    [Serializable]
    public class BubbleConfig
    {
        public int numPerRow;
        public float size;
        public float shotColliderScale;
    }

    public BubbleConfig bubbles;
}
