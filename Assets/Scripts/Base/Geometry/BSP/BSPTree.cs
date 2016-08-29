using UnityEngine;
using System.Collections.Generic;

namespace Geometry.BSP
{
    sealed public class BSPTree
    {
        public BSPNode Root { get; private set; }

        public BSPTree(Polygon polygon)
        {
            Root = new BSPNode(polygon);
        }

        public List<Polygon> GetFrontLeaves()
        {
            return Root.IsLeaf ? new List<Polygon> { Root.Polygon } : GetFrontLeaves(new List<Polygon>(), Root);
        }

        public void Subtract(Polygon polygon)
        {
            var result = false;

            for (int index = 0, count = polygon.Indices.Length; index < count; index++)
            {
                result |= Root.Split(
                    polygon.Vertices[polygon.Indices[(index + 1) % count]],
                    polygon.Vertices[polygon.Indices[index]]
                );
            }

            // Entire polygon was outside of the visible area
            if (!result && Root.IsLeaf)
            {
                Root.Cull();
            }
        }

        private List<Polygon> GetFrontLeaves(List<Polygon> leaves, BSPNode node)
        {
            if (node.Front != null)
            {
                if (node.Front.IsLeaf)
                {
                    leaves.Add(node.Front.Polygon);
                }

                GetFrontLeaves(leaves, node.Front);
            }

            if (node.Back != null)
            {
                GetFrontLeaves(leaves, node.Back);
            }

            return leaves;
        }
    }
}
