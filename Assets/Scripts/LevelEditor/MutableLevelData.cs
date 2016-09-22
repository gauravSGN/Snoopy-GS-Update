using Model;
using System.Linq;
using Snoopy.Model;
using System.Collections.Generic;

namespace LevelEditor
{
    public class MutableLevelData : LevelData
    {
        public new string Background
        {
            get { return base.Background; }
            set { background = value; }
        }

        public new int ShotCount
        {
            get { return base.ShotCount; }
            set { shotCount = value; }
        }

        public new float[] PowerUpFills
        {
            get { return base.PowerUpFills; }
            set { powerUpFills = value; }
        }

        public new IEnumerable<PuzzleData> Puzzles
        {
            get { return base.Puzzles; }
            set { puzzles = value.ToArray(); }
        }

        public new int[] StarValues
        {
            get { return base.StarValues; }
            set { starValues = value; }
        }

        public new float StarMultiplier
        {
            get { return base.StarMultiplier; }
            set { starMultiplier = value; }
        }

        public new BubbleQueueDefinition Queue
        {
            get { return base.Queue; }
            set { queue = value; }
        }

        public new RandomBubbleDefinition[] Randoms
        {
            get { return base.Randoms; }
            set { randoms = value; }
        }

        public new LevelModifierData[] Modifiers
        {
            get { return base.Modifiers; }
            set { modifiers = value; }
        }
    }
}
