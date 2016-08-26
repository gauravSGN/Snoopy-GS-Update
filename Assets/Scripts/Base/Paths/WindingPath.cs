using UnityEngine;
using System.Collections.Generic;

namespace Paths
{
    sealed public class WindingPath
    {
        private class ControlPoint
        {
            public Vector3 position;
            public Vector3 tangent;
        }

        private class Segment
        {
            private Vector3[] controls = new Vector3[4];

            public int Index { get; private set; }
            public float Length { get; private set; }
            public float Position { get; private set; }

            public void Initialize(List<ControlPoint> points, int index)
            {
                Index = index;
                Position = 0.0f;
                controls[0] = points[index].position;
                controls[3] = points[index + 1].position;
                Length = (controls[3] - controls[0]).magnitude;

                controls[1] = controls[0] + points[index].tangent * (Length / 3.0f);
                controls[2] = controls[3] - points[index + 1].tangent * (Length / 3.0f);
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
                var t = Position / Length;
                var inv = 1.0f - t;

                return controls[0] * inv * inv * inv +
                       controls[1] * 3.0f * t * inv * inv +
                       controls[2] * 3.0f * t * t * inv +
                       controls[3] * t * t * t;
            }
        }

        private readonly List<ControlPoint> points = new List<ControlPoint>();
        private readonly Segment current = new Segment();

        public bool Complete { get; private set; }

        public WindingPath(Vector3 start, Vector3 end)
        {
            points.Add(new ControlPoint { position = start });
            points.Add(new ControlPoint { position = end });

            Subdivide();
            ComputeTangents();

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

        private void Subdivide()
        {
            var midpoint = (points[0].position + points[1].position) / 2.0f;
            points.Insert(1, new ControlPoint { position = midpoint });
        }

        private void ComputeTangents()
        {
            var count = points.Count;

            points[0].tangent = GenerateTangent(points[0].position, points[1].position);
            points[count - 1].tangent = GenerateTangent(points[count - 2].position, points[count - 1].position);

            for (var i = 1; i < count - 1; i++)
            {
                points[i].tangent = GenerateTangent(points[i - 1].position, points[i + 1].position);
            }
        }

        private Vector3 GenerateTangent(Vector3 start, Vector3 end)
        {
            var normal = (end - start).normalized;
            var rotation = Quaternion.AngleAxis(Random.Range(-45.0f, 45.0f), Vector3.forward);

            return rotation * normal;
        }
    }
}
