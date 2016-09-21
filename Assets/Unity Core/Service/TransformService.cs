using Registry;
using UnityEngine;

namespace Service
{
    public interface TransformService : SharedService
    {
        void Register(Transform transform, TransformUsage usage);
        void Unregister(Transform transform, TransformUsage usage);

        Transform Get(TransformUsage usage);
        T Get<T>(TransformUsage usage) where T : Transform;
    }
}
