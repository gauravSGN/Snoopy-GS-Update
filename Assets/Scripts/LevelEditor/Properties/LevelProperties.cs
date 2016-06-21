using Model;

namespace LevelEditor.Properties
{
    public sealed class LevelProperties : Observable, PropertyProxy
    {
        public int ShotCount { get; set; }

        public void FromLevelData(LevelData data)
        {
            ShotCount = data.ShotCount;
        }

        public void ToLevelData(MutableLevelData data)
        {
            data.ShotCount = ShotCount;
        }
    }
}
