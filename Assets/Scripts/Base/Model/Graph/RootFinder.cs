using System.Collections.Generic;

namespace Graph
{
    public class RootFinder
    {
        private readonly List<IGraphElement> visited = new List<IGraphElement>();
        private int count;

        public IEnumerable<IGraphElement> Visited
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

        public bool Contains(IGraphElement node)
        {
            return visited.Contains(node) && (visited.IndexOf(node) < count);
        }

        public bool IsConnectedToRoot(IGraphElement node)
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

        private void AddVisitedNode(IGraphElement node)
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
