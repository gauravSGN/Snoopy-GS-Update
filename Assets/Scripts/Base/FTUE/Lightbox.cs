using Util;
using Geometry;
using UnityEngine;
using System.Linq;
using Geometry.BSP;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FTUE
{
    sealed public class Lightbox : Graphic, ICanvasRaycastFilter
    {
        [System.Serializable]
        public struct PolyInfo
        {
            public List<Vector2> vertices;
        }

        [SerializeField]
        private List<PolyInfo> polygons = new List<PolyInfo>();

        private readonly List<int> triangles = new List<int>();
        private List<Vector2> vertices;

        public List<PolyInfo> Polygons { get { return polygons; } }

        public void Validate()
        {
            SetVerticesDirty();
        }

        public void AddCutout(PolyInfo polygon)
        {
            polygons.Add(polygon);
            Validate();
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Vector2 localPoint;

            var inside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                 transform as RectTransform,
                 sp,
                 eventCamera,
                 out localPoint
             );

            if (inside)
            {
                for (int index = 0, count = triangles.Count; index < count; index += 3)
                {
                    var p0 = vertices[triangles[index]];
                    var p1 = vertices[triangles[index + 1]];
                    var p2 = vertices[triangles[index + 2]];

                    var b0 = (localPoint.x - p1.x) * (p0.y - p1.y) - (p0.x - p1.x) * (localPoint.y - p1.y) < 0.0f;
                    var b1 = (localPoint.x - p2.x) * (p1.y - p2.y) - (p1.x - p2.x) * (localPoint.y - p2.y) < 0.0f;
                    var b2 = (localPoint.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (localPoint.y - p0.y) < 0.0f;

                    if ((b0 == b1) && (b1 == b2))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        override protected void OnPopulateMesh(VertexHelper vh)
        {
            var polyList = new List<Polygon> { GetStartingPolygon() };

            foreach (var poly in polygons)
            {
                if (poly.vertices.Count > 2)
                {
                    var polygon = new Polygon(poly.vertices, MathUtil.Range(poly.vertices.Count).ToArray());
                    polyList = polyList.SelectMany(p => SubtractPolygon(p, polygon))
                                       .Where(p => p.Indices.Length > 2)
                                       .ToList();
                }
            }

            triangles.Clear();

            foreach (var polygon in polyList)
            {
                for (int index = 1, count = polygon.Indices.Length - 1; index < count; index++)
                {
                    triangles.Add(polygon.Indices[0]);
                    triangles.Add(polygon.Indices[index]);
                    triangles.Add(polygon.Indices[index + 1]);
                }
            }

            vh.Clear();

            if (polyList.Count > 0)
            {
                vertices = polyList[0].Vertices;

                foreach (var vertex in vertices)
                {
                    vh.AddVert(new UIVertex { position = vertex });
                }

                for (int index = 0, count = triangles.Count; index < count; index += 3)
                {
                    vh.AddTriangle(triangles[index], triangles[index + 1], triangles[index + 2]);
                }
            }
        }

        private Polygon GetStartingPolygon()
        {
            var rectTransform = GetComponent<RectTransform>();
            var rect = rectTransform.rect;

            var baseVertices = new[]
                {
                    new Vector2(rect.xMin, rect.yMax),
                    new Vector2(rect.xMax, rect.yMax),
                    new Vector2(rect.xMax, rect.yMin),
                    new Vector2(rect.xMin, rect.yMin),
                };

            return new Polygon(baseVertices, MathUtil.Range(4).ToArray());
        }

        private List<Polygon> SubtractPolygon(Polygon start, Polygon toRemove)
        {
            var tree = new BSPTree(start);
            tree.Subtract(toRemove);
            return tree.GetFrontLeaves();
        }
    }
}
