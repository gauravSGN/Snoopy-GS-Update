using UnityEngine;
using System.Collections.Generic;

namespace Paths
{
    sealed public class WindingPath
    {
        private class Segment
        {
            private Vector3 start;
            private Vector3 end;

            public int Index { get; private set; }
            public float Length { get; private set; }
            public float Position { get; private set; }

            public void Initialize(List<Vector3> points, int index)
            {
                Index = index;
                Position = 0.0f;
                start = points[index];
                end = points[index + 1];
                Length = (end - start).magnitude;
            }

            public float Advance(float distance)
            {
                var delta = Mathf.Min(distance, Length - Position);
                var remainder = distance - delta;

                Position += delta;

                return remainder;
            }

            public Vector3 Evaluate()
            {
                return start + (end - start) * (Position / Length);
            }
        }

        private readonly List<Vector3> points = new List<Vector3>();
        private readonly Segment current = new Segment();

        public bool Complete { get; private set; }

        public WindingPath(Vector3 start, Vector3 end)
        {
            points.Add(start);
            points.Add(end);

            current.Initialize(points, 0);
        }

        public Vector3 Advance(float distance)
        {
            var remainder = current.Advance(distance);

            if (current.Position >= current.Length)
            {
                if (current.Index < (points.Count - 2))
                {
                    current.Initialize(points, current.Index + 1);
                    return Advance(remainder);
                }
                else
                {
                    Complete = true;
                }
            }

            return current.Evaluate();
        }
    }
}
