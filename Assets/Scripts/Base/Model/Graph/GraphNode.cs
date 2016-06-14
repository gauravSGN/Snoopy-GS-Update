using System.Collections.Generic;

namespace Graph
{
    public interface GraphNode
    {
        bool IsRoot { get; }
        uint DistanceFromRoot { get; set; }
        IEnumerable<GraphNode> Neighbors { get; }

        void DisconnectAll();
        void RemoveFromGraph();
    }
}
