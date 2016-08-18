using Service;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Snoopy.Characters
{
    sealed public class WoodstockEventHandler : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private GameObject celebration;

        private Transform gameView;

        public Bubble Model { get; set; }
        public Vector3 LandingSpot { get; set; }

        public void Start()
        {
            gameView = GameObject.Find("Game View").transform;

            Model.OnPopped += OnPoppedHandler;
            Model.OnDisconnected += OnPoppedHandler;

            var eventService = GlobalState.EventService;
            eventService.AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
            eventService.AddEventHandler<ShotsRemainingEvent>(OnShotsRemaining);
        }

        public void OnDestroy()
        {
            Model.OnPopped -= OnPoppedHandler;
            Model.OnDisconnected -= OnPoppedHandler;
        }

        private void OnPoppedHandler(Bubble bubble)
        {
            transform.SetParent(gameView, true);
            animator.SetBool("HasEscaped", true);

            StartCoroutine(FlyToGround());

            if (celebration != null)
            {
                Instantiate(celebration, transform.position, Quaternion.identity);
            }
        }

        private void OnLevelComplete(LevelCompleteEvent gameEvent)
        {
            animator.SetBool("WonLevel", gameEvent.Won);
            animator.SetBool("LostLevel", !gameEvent.Won);
        }

        private void OnShotsRemaining(ShotsRemainingEvent gameEvent)
        {
            if (gameEvent.shots == 10)
            {
                animator.SetBool("LosingLevel", true);
            }
        }

        private IEnumerator FlyToGround()
        {
            var myTransform = transform;
            var delta = (myTransform.localPosition - LandingSpot);
            var distance = delta.magnitude;
            delta /= distance;
            var speed = GlobalState.Instance.Config.woodstock.flightSpeed;

            while (distance > 0.0f)
            {
                distance = Mathf.Max(0.0f, distance - Time.deltaTime * speed);
                myTransform.localPosition = LandingSpot + delta * distance;
                yield return null;
            }

            animator.SetBool("OnGround", true);
        }
    }
}
