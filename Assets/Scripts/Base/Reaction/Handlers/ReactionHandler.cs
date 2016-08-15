using System.Collections;

namespace Reaction
{
    public interface ReactionHandler
    {
        int Count { get; }
        IEnumerator HandleActions();
    }
}
