using System.Collections;

namespace Reaction
{
    public interface ReactionHandler
    {
        int Count { get; }
        void Setup(ReactionPriority priority);
        IEnumerator HandleActions();
    }
}
