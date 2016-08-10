using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Geometry.Bsp
{
    sealed public class BspNode
    {
        public Polygon Polygon { get; private set; }
        public BspNode Front { get; private set; }
        public BspNode Back { get; private set; }
        public bool IsLeaf { get { return (Front == null) && (Back == null); } }

        public BspNode(Polygon polygon)
        {
            Polygon = polygon;
        }

        public void Cull()
        {
            Front = new BspNode(new Polygon(Polygon.Vertices, new int[0]));
            Back = new BspNode(new Polygon(Polygon.Vertices, Polygon.Indices));
        }

        public bool Split(Vector2 start, Vector2 end)
        {
            if ((Front != null) && (Back != null))
            {
                return Front.Split(start, end) | Back.Split(start, end);
            }

            bool allInside = true;
            int insideCount = 0;

            for (int index = 0, count = Polygon.Indices.Length; index < count; index++)
            {
                var firstVertex = Polygon.Vertices[Polygon.Indices[index]];
                var secondVertex = Polygon.Vertices[Polygon.Indices[(index + 1) % count]];

                var first = LineSegment.Classify(firstVertex, secondVertex, start);
                var second = LineSegment.Classify(firstVertex, secondVertex, end);

                first = (first == LineSegment.PointClassification.Colinear) ? second : first;
                second = (second == LineSegment.PointClassification.Colinear) ? first : second;

                if (first != LineSegment.PointClassification.Colinear)
                {
                    allInside = allInside && (first == second) && (first == LineSegment.PointClassification.Inside);

                    if ((first == second) && (first == LineSegment.PointClassification.Outside))
                    {
                        return LineSegment.Classify(start, end, firstVertex) == LineSegment.PointClassification.Inside;
                    }

                    ++insideCount;

                    if (first != second)
                    {
                        return SplitSegmentAndRetry(firstVertex, secondVertex, start, end);
                    }
                }
            }

            if (allInside && (insideCount > 0))
            {
                DividePolygon(start, end);
                return true;
            }

            return false;
        }

        private bool SplitSegmentAndRetry(Vector2 polyStart, Vector2 polyEnd, Vector2 start, Vector2 end)
        {
            var point = LineSegment.Intersect(polyStart, polyEnd, start, end);
            return Split(start, point) | Split(point, end);
        }

        private void DividePolygon(Vector2 start, Vector2 end)
        {
            var startIndex = FindDivisionPoint(start, end);
            var endIndex = FindDivisionPoint(end, start);

            var firstVertex = Polygon.Vertices[Polygon.Indices[startIndex]];
            var secondVertex = Polygon.Vertices[Polygon.Indices[(startIndex + 1) % Polygon.Indices.Length]];
            var firstIntersect = LineSegment.Intersect(firstVertex, secondVertex, start, end);

            firstVertex = Polygon.Vertices[Polygon.Indices[endIndex]];
            secondVertex = Polygon.Vertices[Polygon.Indices[(endIndex + 1) % Polygon.Indices.Length]];
            var secondIntersect = LineSegment.Intersect(firstVertex, secondVertex, end, start);

            var inside = CreateSubPolygon(WalkEdges(startIndex + 1, endIndex), secondIntersect, start, end, firstIntersect);
            var outside = CreateSubPolygon(WalkEdges(endIndex + 1, startIndex), firstIntersect, end, start, secondIntersect);

            Front = new BspNode(new Polygon(Polygon.Vertices, inside.ToArray()));
            Back = new BspNode(new Polygon(Polygon.Vertices, outside.ToArray()));
        }

        private int FindDivisionPoint(Vector2 start, Vector2 end)
        {
            var delta2 = end - start;

            for (int index = 0, count = Polygon.Indices.Length; index < count; index++)
            {
                var nextIndex = (index + 1) % count;
                var delta1 = Polygon.Vertices[Polygon.Indices[nextIndex]] - Polygon.Vertices[Polygon.Indices[index]];
                var product = delta1.Cross(delta2);

                if (Mathf.Abs(product) > Mathf.Epsilon)
                {
                    var delta = (start - Polygon.Vertices[Polygon.Indices[index]]);

                    var value1 = delta.Cross(delta2) / product;
                    var value2 = delta.Cross(delta1) / product;

                    if ((value2 > 0.0f) && (value1 >= 0.0f) && (value1 <= 1.0f))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }

        private List<int> CreateSubPolygon(List<int> indices, params Vector2[] points)
        {
            for (int index = 0, count = points.Length; index < count; index++)
            {
                var point = points[index];
                var nextIndex = Polygon.Vertices.Count;

                if (!Polygon.Vertices.Contains(point))
                {
                    Polygon.Vertices.Add(point);
                }
                else
                {
                    nextIndex = Polygon.Vertices.IndexOf(point);
                }

                indices.Add(nextIndex);
            }

            return SimplifyPolygon(indices);
        }

        private List<int> SimplifyPolygon(List<int> indices)
        {
            var index = 0;

            while (index < (indices.Count - 1))
            {
                if (indices[index] == indices[index + 1])
                {
                    indices.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }

            return indices;
        }

        private List<int> WalkEdges(int startIndex, int endIndex)
        {
            var result = new List<int>();
            var count = Polygon.Indices.Length;
            var current = (startIndex + count - 1) % count;

            do
            {
                current = (current + 1) % count;
                result.Add(Polygon.Indices[current]);
            } while (current != endIndex);

            return result;
        }
    }
}
