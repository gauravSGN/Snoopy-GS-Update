using UnityEngine;
using System.Collections.Generic;

namespace Geometry.Bsp
{
    sealed public class BspTree
    {
        private BspNode root;

        public BspNode Root { get { return root; } }

        public BspTree(Polygon polygon)
        {
            root = new BspNode(polygon);
        }

        public List<Polygon> GetFrontLeaves()
        {
            if (root.IsLeaf)
            {
                return new List<Polygon> { root.Polygon };
            }
            else
            {
                return GetFrontLeaves(new List<Polygon>(), root);
            }
        }

        public void Subtract(Polygon polygon)
        {
            var result = false;

            for (int index = 0, count = polygon.Indices.Length; index < count; index++)
            {
                result |= root.Split(
                    polygon.Vertices[polygon.Indices[(index + 1) % count]],
                    polygon.Vertices[polygon.Indices[index]]
                );
            }

            // Entire polygon was outside of the visible area
            if (!result && root.IsLeaf)
            {
                root.Cull();
            }
        }

        private List<Polygon> GetFrontLeaves(List<Polygon> leaves, BspNode node)
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
