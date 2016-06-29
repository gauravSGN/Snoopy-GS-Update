using Model;

namespace LevelEditor.Properties
{
    sealed public class LevelProperties : Observable, PropertyProxy
    {
        public int ShotCount { get; set; }
        public int[] StarValues { get; set; }

        public LevelProperties()
        {
            StarValues = new int[3];
        }

        public void FromLevelData(LevelData data)
        {
            ShotCount = data.ShotCount;
            StarValues = data.StarValues;
        }

        public void ToLevelData(MutableLevelData data)
        {
            data.ShotCount = ShotCount;
            data.StarValues = StarValues;
        }
    }
}
