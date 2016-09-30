using Model;
using System.Linq;
using System.Collections.Generic;

namespace LevelEditor
{
    public class MutablePuzzleData : PuzzleData
    {
        public new IEnumerable<BubbleData> Bubbles
        {
            get { return base.Bubbles; }
            set { bubbles = (value != null) ? value.ToList() : null; }
        }
    }
}
