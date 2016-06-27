using UnityEngine;
using System.Collections;

namespace Effects
{
    public static class ImpactShockwaveEffect
    {
        public static IEnumerator Play(GameObject bubble, Vector3 origin)
        {
            var config = GlobalState.Instance.Config.reactions;
            var delta = bubble.transform.position - origin;
            var distance = delta.magnitude;
            var direction = delta / distance;
            var startPosition = bubble.transform.position;

            var magnitude = config.shockwaveDistance * Mathf.Cos(distance / config.shockwaveRadius * Mathf.PI / 2.0f);
            var duration = (magnitude / config.shockwaveSpeed) * Mathf.PI;
            var startTime = Time.time + distance / config.shockwaveSpeed;
            var endTime = startTime + duration;

            var currentTime = Time.time;

            while (currentTime < endTime)
            {
                var current = Mathf.Clamp01((currentTime - startTime) / duration);
                var power = Mathf.Sin(current * Mathf.PI) * magnitude;

                bubble.transform.position = startPosition + direction * power;

                yield return null;
                currentTime = Time.time;
            }

            bubble.transform.position = startPosition;
        }
    }
}
