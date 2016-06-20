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
            set { shot_count = value; }
        }

        public new float[] PowerUpFills
        {
            get { return base.PowerUpFills; }
            set { power_up_fills = value; }
        }

        public new IEnumerable<BubbleData> Bubbles
        {
            get { return base.Bubbles; }
            set { bubbles = value.ToArray(); }
        }
    }
}
