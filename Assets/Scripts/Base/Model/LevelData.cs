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
        virtual public IEnumerable<BubbleData> Bubbles { get { return bubbles; } }
        virtual public int[] StarValues { get { return starValues; } }
        virtual public BubbleQueueDefinition Queue { get { return queue; } }
        virtual public RandomBubbleDefinition[] Randoms { get { return randoms; } }

        public List<LevelGoal> goals;

        [SerializeField]
        protected string background;

        [SerializeField]
        protected int shotCount;

        [SerializeField]
        protected float[] powerUpFills;

        [SerializeField]
        protected BubbleData[] bubbles;

        [SerializeField]
        protected int[] starValues = new int[3];

        [SerializeField]
        protected BubbleQueueDefinition queue;

        [SerializeField]
        protected RandomBubbleDefinition[] randoms;
    }
}
