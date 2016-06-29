namespace Service
{
    public interface SceneService : SharedService
    {
        string NextLevelData { get; set; }
        string ReturnScene { get; set; }
    }
}
