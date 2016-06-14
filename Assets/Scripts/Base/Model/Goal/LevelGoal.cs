using UnityEngine;
using System;

namespace Goal
{
    [Serializable]
    public abstract class LevelGoal : Observable
    {
        public abstract GoalType Type { get; }
        public int CurrentValue { get { return currentValue; } }
        public int TargetValue { get { return targetValue; } }

        [SerializeField]
        protected int currentValue;

        [SerializeField]
        protected int targetValue;

        public abstract void Initialize(LevelData levelData);
    }
}
