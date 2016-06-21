using Model;

namespace LevelEditor.Properties
{
    public interface PropertyProxy
    {
        void FromLevelData(LevelData data);
        void ToLevelData(MutableLevelData data);
    }
}
