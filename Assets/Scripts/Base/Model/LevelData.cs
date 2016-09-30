using System;
using System.Collections.Generic;
using UnityEngine;
using Goal;
using Snoopy.Model;

namespace Model
{
    [Serializable]
    public class LevelData
    {
        virtual public string Background { get { return background; } }
        virtual public int ShotCount { get { return shotCount; } }
        virtual public float[] PowerUpFills { get { return powerUpFills; } }
        virtual public PuzzleData[] Puzzles { get { return puzzles; } }
        virtual public IEnumerable<BubbleData> Bubbles { get { return puzzles[currentPuzzle].Bubbles; } }
        virtual public int[] StarValues { get { return starValues; } }
        virtual public float StarMultiplier { get { return starMultiplier; } }
        virtual public BubbleQueueDefinition Queue { get { return queue; } }
        virtual public RandomBubbleDefinition[] Randoms { get { return randoms; } }
        virtual public LevelModifierData[] Modifiers { get { return modifiers; } }

        public int currentPuzzle;
        public List<LevelGoal> goals;

        [SerializeField]
        protected string background;

        [SerializeField]
        protected int shotCount;

        [SerializeField]
        protected float[] powerUpFills;

        [SerializeField]
        protected PuzzleData[] puzzles;

        [SerializeField]
        protected int[] starValues = new int[3];

        [SerializeField]
        protected float starMultiplier = 1.0f;

        [SerializeField]
        protected BubbleQueueDefinition queue;

        [SerializeField]
        protected RandomBubbleDefinition[] randoms;

        [SerializeField]
        protected LevelModifierData[] modifiers;
    }
}
