using System;
using System.Collections.Generic;

namespace Graph
{
    public static class GraphUtil
    {
        public static class SearchDelegate<T, ReturnType> where T : GraphElement<T>
        {
            public delegate bool SearchMethod(T instance, ref ReturnType value);
        }

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
                RemoveVisitedNodes(finder, adjacent);

                if (!connected)
                {
                    CullVisitedNodes(finder, adjacent);
                }
            }
        }

        public static ReturnType SearchBreadthFirst<T, ReturnType>(T origin, ReturnType startValue,
                                                                   SearchDelegate<T, ReturnType>.SearchMethod strategy)
            where T : GraphElement<T>
        {
            var value = startValue;
            var nodes = new List<T> { origin };
            var startIndex = 0;
            int count = nodes.Count;

            while (startIndex < count)
            {
                for (var index = startIndex; index < count; index++)
                {
                    if (!strategy(nodes[index], ref value))
                    {
                        return value;
                    }

                    foreach (T neighbor in nodes[index].Neighbors)
                    {
                        if (!nodes.Contains(neighbor))
                        {
                            nodes.Add(neighbor);
                        }
                    }
                }

                startIndex = count;
                count = nodes.Count;
            }

            return value;
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
            foreach (var node in finder.Visited)
            {
                node.RemoveFromGraph();
            }
        }
    }
}
