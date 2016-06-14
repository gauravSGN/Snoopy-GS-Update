using System.Collections.Generic;

namespace Graph
{
    public class RootFinder
    {
        private readonly List<GraphNode> visited = new List<GraphNode>();
        private int count;

        public IEnumerable<GraphNode> Visited
        {
            get
            {
                for (var index = 0; index < count; index++)
                {
                    yield return visited[index];
                }
            }
        }

        public void Reset()
        {
            count = 0;
        }

        public bool Contains(GraphNode node)
        {
            return visited.Contains(node) && (visited.IndexOf(node) < count);
        }

        public bool IsConnectedToRoot(GraphNode node)
        {
            AddVisitedNode(node);

            var result = node.IsRoot;

            if (!result)
            {
                foreach (var neighbor in node.Neighbors)
                {
                    if (!Contains(neighbor) && IsConnectedToRoot(neighbor))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private void AddVisitedNode(GraphNode node)
        {
            if (count >= visited.Count)
            {
                visited.Add(null);
            }

            visited[count] = node;
            count++;
        }
    }
}
