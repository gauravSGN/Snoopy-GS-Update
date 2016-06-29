using System;
using System.Collections.Generic;

namespace Graph
{
    public static class GraphUtil
    {
        public static List<T> MatchNeighbors<T>(List<T> matches, T node, Func<T, bool> predicate) where T : GraphNode
        {
            foreach (T neighbor in node.Neighbors)
            {
                if (!matches.Contains(neighbor) && predicate(neighbor))
                {
                    matches.Add(neighbor);
                    MatchNeighbors(matches, neighbor, predicate);
                }
            }

            return matches;
        }

        public static List<T> GetAdjacentNodes<T>(List<T> nodes) where T : GraphNode
        {
            var surrounding = new List<T>();

            foreach (var node in nodes)
            {
                foreach (T neighbor in node.Neighbors)
                {
                    if (!nodes.Contains(neighbor) && !surrounding.Contains(neighbor))
                    {
                        surrounding.Add(neighbor);
                    }
                }
            }

            return surrounding;
        }

        public static void RemoveNodes<T>(List<T> nodes) where T : GraphElement<T>
        {
            var adjacent = GetAdjacentNodes(nodes);

            foreach (var node in nodes)
            {
                node.DisconnectAll();
            }

            var finder = new RootFinder();
            while (adjacent.Count > 0)
            {
                finder.Reset();
                var connected = finder.IsConnectedToRoot(adjacent[0]);

                if (connected)
                {
                    RemoveVisitedNodes(finder, adjacent);
                }
                else
                {
                    CullVisitedNodes(finder, adjacent);
                }
            }
        }

        private static void RemoveVisitedNodes<T>(RootFinder finder, List<T> adjacent) where T : GraphElement<T>
        {
            for (var index = adjacent.Count - 1; index >= 0; index--)
            {
                if (finder.Contains(adjacent[index]))
                {
                    adjacent.RemoveAt(index);
                }
            }
        }

        private static void CullVisitedNodes<T>(RootFinder finder, List<T> adjacent) where T : GraphElement<T>
        {
            RemoveVisitedNodes(finder, adjacent);

            foreach (var node in finder.Visited)
            {
                node.RemoveFromGraph();
            }
        }
    }
}
