using Util;
using Geometry;
using UnityEngine;
using System.Linq;
using Geometry.BSP;
using System.Collections.Generic;

namespace FTUE
{
    [ExecuteInEditMode]
    sealed public class Lightbox : MonoBehaviour
    {
        [System.Serializable]
        public struct PolyInfo
        {
            public List<Vector2> vertices;
        }

        [SerializeField]
        private Color color;

        [SerializeField]
        private Material material;

        [SerializeField]
        private List<PolyInfo> polygons = new List<PolyInfo>();

        public List<PolyInfo> Polygons { get { return polygons; } }

        public void Start()
        {
            Rebuild();
        }

        public void OnValidate()
        {
            if (enabled)
            {
                Rebuild();
            }
        }

        public void OnEnable()
        {
            OnValidate();
        }

        public void OnDisable()
        {
            GetComponent<CanvasRenderer>().Clear();
        }

        public void AddCutout(PolyInfo polygon)
        {
            polygons.Add(polygon);
            Rebuild();
        }

        private void Rebuild()
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

            var triangles = new List<int>();

            foreach (var polygon in polyList)
            {
                for (int index = 1, count = polygon.Indices.Length - 1; index < count; index++)
                {
                    triangles.Add(polygon.Indices[0]);
                    triangles.Add(polygon.Indices[index]);
                    triangles.Add(polygon.Indices[index + 1]);
                }
            }

            var canvasRenderer = GetComponent<CanvasRenderer>();

            canvasRenderer.Clear();

            if (polyList.Count > 0)
            {
                material.SetColor("_Color", color);
                canvasRenderer.SetMaterial(material, null);
                canvasRenderer.SetMesh(new Mesh
                {
                    vertices = polyList[0].Vertices.Select(v => (Vector3)v).ToArray(),
                    triangles = triangles.ToArray(),
                });
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
