using UnityEngine;
using System.Collections;

namespace Effects
{
    public static class ImpactShockwaveEffect
    {
        public static IEnumerator Play(GameObject bubble, Vector3 origin)
        {
            var config = GlobalState.Instance.Config.impactEffect;
            var ease = config.easing;
            var delta = bubble.transform.position - origin;
            var direction = delta.normalized;
            var startPosition = bubble.transform.position;
            var runTime = 0f;

            while (runTime <= config.duration)
            {
                bubble.transform.position = startPosition + direction * (config.distance * ease.Evaluate(runTime));

                yield return null;
                runTime += Time.deltaTime;
            }

            bubble.transform.position = startPosition;
        }
    }
}
