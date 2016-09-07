using UnityEngine;
using System.Collections.Generic;

namespace Geometry
{
    public class Polygon
    {
        public List<Vector2> Vertices { get; private set; }
        public int[] Indices { get; private set; }

        public Polygon(List<Vector2> vertices, int[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }

        public Polygon(IEnumerable<Vector2> vertices, int[] indices) : this(new List<Vector2>(vertices), indices) {}
    }
}
