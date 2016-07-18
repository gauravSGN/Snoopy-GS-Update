namespace Util
{
    public static class MathUtil
    {
        public const float COS_30_DEGREES = 0.8660254038f;

        public static int CompareInts(int val1, int val2)
        {
            return (val1 - val2) - (val2 - val1);
        }
    }
}
