using UnityEngine;
using System.Collections;
using Animation;
using Service;

namespace Effects
{
    public static class DeathAnimationEffect
    {
        public static IEnumerator Play(GameObject bubble, AnimationType type)
        {
            yield return null;
            var animation = GlobalState.Instance.Services.Get<AnimationService>().CreateByType(type);
            animation.transform.parent = bubble.transform;
            animation.transform.localPosition = Vector3.back;
        }
    }
}

