using System.Collections.Generic;

namespace Graph
{
    public interface IGraphElement
    {
        bool IsRoot { get; }
        uint DistanceFromRoot { get; set; }
        IEnumerable<IGraphElement> Neighbors { get; }

        void DisconnectAll();
        void RemoveFromGraph();
    }
}
