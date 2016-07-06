using UnityEngine;
using Animation;

namespace Service
{
    public interface AnimationService : SharedService
    {
        GameObject CreateByType(AnimationType type);
    }
}