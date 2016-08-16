using UnityEngine;
using System;
using System.Collections;
using Animation;
using Service;

namespace Effects
{
    public static class BlockingAnimationEffect
    {
        public static IEnumerator Play(GameObject bubble, AnimationType type)
        {
            yield return null;

            var animation = GlobalState.Instance.Services.Get<AnimationService>().CreateByType(type);
            var transform = animation.transform;

            transform.SetParent(bubble.transform);
            transform.localPosition = Vector3.back;
            var animationEvents = animation.GetComponent<BlockingAnimationEvent>();

            if (animationEvents != null)
            {
                var blocked = true;

                Action triggerFunction = () =>
                {
                    blocked = false;
                };

                animationEvents.StopBlocking += triggerFunction;

                while (blocked)
                {
                    yield return null;
                }

                animationEvents.StopBlocking -= triggerFunction;
            }
        }
    }
}

