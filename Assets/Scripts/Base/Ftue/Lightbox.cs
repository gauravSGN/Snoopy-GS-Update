using Geometry;
using Geometry.Bsp;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Ftue
{
    sealed public class Lightbox : MonoBehaviour
    {
        [SerializeField]
        private Color color;

        [SerializeField]
        private Material material;

        private readonly List<Polygon> polys = new List<Polygon>();

        void Start()
        {
            Rebuild();
        }

        public void AddCutout(Polygon polygon)
        {
            polys.Add(polygon);
            Rebuild();
        }

        private void Rebuild()
        {
            var polygons = new List<Polygon> { GetStartingPolygon() };

            foreach (var poly in polys)
            {
                polygons = polygons.SelectMany(p => SubtractPolygon(p, poly)).Where(p => p.Indices.Length > 2).ToList();
            }

            var triangles = new List<int>();

            foreach (var polygon in polygons)
            {
                for (int index = 1, count = polygon.Indices.Length - 1; index < count; index++)
                {
                    triangles.Add(polygon.Indices[0]);
                    triangles.Add(polygon.Indices[index]);
                    triangles.Add(polygon.Indices[index + 1]);
                }
            }

            var canvasRenderer = GetComponent<CanvasRenderer>();

            material.SetColor("_Color", color);
            canvasRenderer.SetMaterial(material, null);
            canvasRenderer.SetMesh(new Mesh
            {
                vertices = polygons[0].Vertices.Select(v => (Vector3)v).ToArray(),
                triangles = triangles.ToArray(),
            });
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

            return new Polygon(baseVertices, new[] { 0, 1, 2, 3 });
        }

        private List<Polygon> SubtractPolygon(Polygon start, Polygon toRemove)
        {
            var tree = new BspTree(start);
            tree.Subtract(toRemove);
            return tree.GetFrontLeaves();
        }
    }
}
