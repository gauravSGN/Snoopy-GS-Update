using System;
using Animation;
using UnityEngine;
using System.Collections;

namespace Effects
{
    public static class AnimationEffect
    {
        public static IEnumerator Play(GameObject parent, AnimationType type)
        {
            yield return null;

            CreateAnimation(parent, type);
        }

        public static IEnumerator PlayBlocking(GameObject parent, AnimationType type)
        {
            yield return null;

            var animation = CreateAnimation(parent, type);
            var animationEvents = animation.GetComponent<BlockingAnimationEvent>();

            if (animationEvents != null)
            {
                var blocked = true;

                Action triggerFunction = () => { blocked = false; };

                animationEvents.StopBlocking += triggerFunction;

                while (blocked)
                {
                    yield return null;
                }

                animationEvents.StopBlocking -= triggerFunction;
            }
        }

        private static GameObject CreateAnimation(GameObject parent, AnimationType type)
        {
            var animation = GlobalState.AnimationService.CreateByType(type);
            var transform = animation.transform;

            transform.SetParent(parent.transform);
            transform.localPosition = Vector3.back;

            return animation;
        }
    }
}

