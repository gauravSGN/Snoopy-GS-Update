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
            var magnitude = delta.magnitude;
            var direction = delta / magnitude;
            var startPosition = bubble.transform.position;
            var distance = config.distance;
            var runTime = 0f;

            distance *= config.distanceScaling.Evaluate((magnitude - bubbleSize) / (config.radius - bubbleSize));

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
