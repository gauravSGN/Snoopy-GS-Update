using UnityEngine;
using System.Collections;
using Registry;
using Service;

namespace Sequence
{
    public class PowerUpReturnSequence : MonoBehaviour
    {
        [SerializeField]
        private TransformUsage usage = TransformUsage.YellowPowerUpReturnTarget;

        [SerializeField]
        private float returnTime;

        private SpriteRenderer itemRenderer;
        private Transform originalParent;

        public void Play()
        {
            originalParent = transform.parent;
            transform.SetParent(null);

            itemRenderer = GetComponent<SpriteRenderer>();
            itemRenderer.enabled = true;

            var eventService = GlobalState.EventService;
            eventService.AddEventHandler<PowerUpReturningEvent>(OnReturning);
            eventService.AddEventHandler<PowerUpReturnedEvent>(OnReturned);

            eventService.Dispatch<PowerUpPrepareForReturnEvent>(new PowerUpPrepareForReturnEvent());
        }

        private void OnReturning()
        {
            GlobalState.EventService.RemoveEventHandler<PowerUpReturningEvent>(OnReturning);
            StartCoroutine(ReturnToTarget(GlobalState.Instance.Services.Get<TransformService>().Get(usage)));
        }

        private void OnReturned()
        {
            GlobalState.EventService.RemoveEventHandler<PowerUpReturnedEvent>(OnReturned);

            transform.SetParent(originalParent);
            transform.localPosition = Vector3.forward;
            itemRenderer.enabled = false;
        }

        private IEnumerator ReturnToTarget(Transform target)
        {
            var runTime = 0f;
            var speed = Vector3.Distance(transform.position, target.position) / returnTime;

            while (runTime <= returnTime)
            {
                var elapsedTime = Time.deltaTime;
                var distance = speed * elapsedTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, distance);

                if (transform.position == target.position)
                {
                    transform.SetParent(target.transform);
                    break;
                }

                yield return null;

                runTime += elapsedTime;
            }
        }
    }
}