using UnityEngine;
using System.Collections.Generic;

namespace Geometry
{
    static public class LineSegment
    {
        public enum PointClassification
        {
            Inside,
            Outside,
            Colinear,
        }

        static public PointClassification Classify(Vector2 start, Vector2 end, Vector2 point)
        {
            var product = (end - start).Cross(point - start);

            if (Mathf.Abs(product) <= 1.0f)
            {
                return PointClassification.Colinear;
            }

            return (product < 0.0f) ? PointClassification.Inside : PointClassification.Outside;
        }

        static public Vector2 Intersect(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            var delta1 = end1 - start1;
            var delta2 = end2 - start2;

            var product = delta1.Cross(delta2);
            if (Mathf.Abs(product) <= Mathf.Epsilon)
            {
                return new Vector2();
            }

            var value = (start2 - start1).Cross(delta1) / product;
            return start2 + delta2 * value;
        }
    }
}
