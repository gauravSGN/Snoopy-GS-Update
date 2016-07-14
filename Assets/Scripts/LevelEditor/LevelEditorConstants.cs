using Util;

namespace LevelEditor
{
    static public class LevelEditorConstants
    {
        public const int UNDO_BUFFER_SIZE = 10;

        // Designers wanted the play area larger and, after shuffling elements around, 22% larger was what fit.
        public const float BUBBLE_SIZE = 32.0f * 1.22f;
        public const float HALF_SIZE = BUBBLE_SIZE / 2.0f;
        public const float ROW_HEIGHT = BUBBLE_SIZE * MathUtil.COS_30_DEGREES;

        public const string RANDOMS_EXTENSION = "json";
        public const string RANDOMS_BASE_PATH = "Data/Randoms";
    }
}
