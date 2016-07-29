using Util;

namespace LevelEditor
{
    static public class LevelEditorConstants
    {
        public const string SCENE_NAME = "LevelEditor";

        public const int UNDO_BUFFER_SIZE = 10;

        // Designers wanted the play area to be even larger, so it has been increased by 60.39593909%.
        public const float BUBBLE_SIZE = 32.0f * 1.6039593909f;
        public const float HALF_SIZE = BUBBLE_SIZE / 2.0f;
        public const float ROW_HEIGHT = BUBBLE_SIZE * MathUtil.COS_30_DEGREES;

        public const string LEVEL_EXTENSION = "json";
        public const string LEVEL_BASE_PATH = "Resources/Levels";

        public const string RANDOMS_EXTENSION = "json";
        public const string RANDOMS_BASE_PATH = "Data/Randoms";

        public const string DEFAULT_BACKGROUND = "Backgrounds/episode_0_background";
    }
}
