namespace Service
{
    public interface SceneService : SharedService
    {
        int LevelNumber { get; set; }
        string NextLevelData { get; set; }
        string ReturnScene { get; set; }

        void Reset();
    }
}
