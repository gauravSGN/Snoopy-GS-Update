namespace Service
{
    public class SceneTransitionData : SceneService
    {
        public int LevelNumber { get; set; }
        public string NextLevelData { get; set; }
        public string ReturnScene { get; set; }

        public void Reset()
        {
            LevelNumber = -1;
            ReturnScene = "";
            NextLevelData = "";
        }
    }
}
