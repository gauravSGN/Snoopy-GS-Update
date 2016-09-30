using UnityEngine;

namespace Paths
{
    public interface Path
    {
        bool Complete { get; }

        Vector3 Advance(float distance);
    }
}
