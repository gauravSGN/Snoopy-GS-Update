using System.Linq;
using System.Collections.Generic;
using Model;

namespace LevelEditor
{
    public class MutableLevelData : LevelData
    {
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

        public new IEnumerable<BubbleData> Bubbles
        {
            get { return base.Bubbles; }
            set { bubbles = value.ToArray(); }
        }

        public new int[] StarValues
        {
            get { return base.StarValues; }
            set { starValues = value; }
        }
    }
}
