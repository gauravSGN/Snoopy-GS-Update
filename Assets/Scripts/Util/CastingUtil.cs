using UnityEngine;
using Model.Scan;
using System.Linq;
using System.Collections.Generic;

namespace Util
{
    public static class CastingUtil
    {
        public delegate RaycastHit2D[] ScanFunction();

        public static RaycastHit2D[] BoundsBoxCast(Bounds bounds, int layers)
        {
            var origin = bounds.center;
            var size = bounds.max - bounds.min;
            var direction = Vector2.left;

            return Physics2D.BoxCastAll(origin, size, 0, direction, 0, layers);
        }

        // Several small casts to find bubbles in a shape specificed by the ScanDefinition.
        public static RaycastHit2D[] RelativeBubbleCast(GameObject baseBubble, ScanDefinition scanLocations)
        {
            var bubbleSize = GlobalState.Instance.Config.bubbles.size;
            var baseSize = bubbleSize * 0.2f;
            var basePosition = baseBubble.transform.position;
            var scans = new List<RaycastHit2D[]>();

            foreach (var location in scanLocations)
            {
                var origin = new Vector2(basePosition.x + (location.x * bubbleSize),
                                        basePosition.y + (location.y * bubbleSize));

                scans.Add(Physics2D.CircleCastAll(origin, baseSize, Vector2.zero, 0.0f));
            }

            return scans.Aggregate(new RaycastHit2D[0], (acc, scan) => acc.Concat(scan).ToArray()).ToArray();
        }
    }
}
