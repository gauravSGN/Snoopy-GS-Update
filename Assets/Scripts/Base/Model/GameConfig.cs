﻿using UnityEngine;
using System;

public class GameConfig : ScriptableObject
{
    [Serializable]
    public class BubbleConfig
    {
        public int numPerRow;
        public float size;
        public float shotColliderScale;
    }

    [Serializable]
    public class ReactionConfig
    {
        public float popSpreadDelay;
    }

    public BubbleConfig bubbles;
    public ReactionConfig reactions;
}
