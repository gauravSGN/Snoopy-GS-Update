using UnityEngine;

namespace Util
{
    public static class CastingUtil
    {
        public static RaycastHit2D[] BoundsBoxCast(Bounds bounds, int layers)
        {
            var origin = bounds.center;
            var size = bounds.max - bounds.min;
            var direction = Vector2.left;

            return Physics2D.BoxCastAll(origin, size, 0, direction, 0, layers);
        }
    }
}
