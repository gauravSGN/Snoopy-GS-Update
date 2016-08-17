using UnityEngine;
using System.Collections;
using Animation;

namespace Effects
{
    public static class AnimationEffect
    {
        public static IEnumerator Play(GameObject bubble, AnimationType type)
        {
            yield return null;

            var animation = GlobalState.AnimationService.CreateByType(type);
            var transform = animation.transform;

            transform.SetParent(bubble.transform);
            transform.localPosition = Vector3.back;
        }
    }
}

