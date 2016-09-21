using UnityEngine;
using Model.Scan;
using System.Linq;
using System.Collections.Generic;

namespace Util
{
    public static class CastingUtil
    {
        public delegate List<RaycastHit2D[]> ScanFunction();

        public static RaycastHit2D[] BoundsBoxCast(Bounds bounds, int layers)
        {
            var origin = bounds.center;
            var size = bounds.max - bounds.min;
            var direction = Vector2.left;

            return Physics2D.BoxCastAll(origin, size, 0, direction, 0, layers);
        }

        // Several small casts to find bubbles in a shape specificed by the ScanDefinition.
        public static List<RaycastHit2D[]> RelativeBubbleCast(GameObject baseBubble, ScanDefinition scanLocations)
        {
            var bubbleSize = GlobalState.Instance.Config.bubbles.size;
            var yBubbleSize = bubbleSize * MathUtil.COS_30_DEGREES;
            var baseSize = bubbleSize * 0.2f;
            var basePosition = baseBubble.transform.position;
            var scans = new List<RaycastHit2D[]>();

            foreach (var scanGroup in scanLocations.locations)
            {
                var scan = new List<RaycastHit2D[]>();

                foreach (var location in scanGroup)
                {
                    var origin = new Vector2(basePosition.x + (location.x * bubbleSize),
                                            basePosition.y + (location.y * yBubbleSize));

                    scan.Add(Physics2D.CircleCastAll(origin, baseSize, Vector2.zero, 0.0f));
                }

                scans.Add(AggregateScans(scan));
            }

            return scans;
        }

        public static List<RaycastHit2D[]> FullRowBubbleCast(GameObject baseBubble, int rowsBelow, int rowsAbove)
        {
            var bubbleSize = GlobalState.Instance.Config.bubbles.size;
            var xOffset = GlobalState.Instance.Config.bubbles.numPerRow * bubbleSize;
            var yOffset = bubbleSize * MathUtil.COS_30_DEGREES;
            var basePosition = baseBubble.transform.position;
            basePosition.y -= rowsBelow * yOffset;

            var start = new Vector2(basePosition.x - xOffset, basePosition.y);
            var end = new Vector2(basePosition.x + xOffset, basePosition.y);
            var scans = new List<RaycastHit2D[]>();

            for (int row = -rowsBelow; row <= rowsAbove; row++)
            {
                scans.Add(Physics2D.LinecastAll(start, end));
                start.y = end.y = start.y + yOffset;
            }

            return scans;
        }

        private static RaycastHit2D[] AggregateScans(List<RaycastHit2D[]> scans)
        {
            return scans.SelectMany(scan => scan).ToArray();
        }
    }
}
