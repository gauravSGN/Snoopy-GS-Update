using UnityEngine;
using System.Collections;

namespace Effects
{
    public static class ImpactShockwaveEffect
    {
        public static IEnumerator Play(GameObject bubble, Vector3 origin)
        {
            var bubbleSize = GlobalState.Instance.Config.bubbles.size;
            var config = GlobalState.Instance.Config.impactEffect;
            var ease = config.easing;
            var delta = bubble.transform.position - origin;
            var direction = delta.normalized;
            var startPosition = bubble.transform.position;
            var distanceScale = config.distanceScaling.Evaluate((delta.magnitude - bubbleSize) / (config.radius - bubbleSize));
            var distance = config.distance * distanceScale;
            var runTime = 0f;

            while (runTime <= config.duration)
            {
                bubble.transform.position = startPosition + direction * (distance * ease.Evaluate(runTime));

                yield return null;
                runTime += Time.deltaTime;
            }

            bubble.transform.position = startPosition;
        }
    }
}
