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
        private GameObject trail;

        [SerializeField]
        private GameObject celebration;

        [SerializeField]
        private AudioClip escapeSound;

        [SerializeField]
        private AudioClip loseSound;

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
            eventService.AddEventHandler<LowMovesEvent>(OnLowMoves);
        }

        public void OnDestroy()
        {
            Model.OnPopped -= OnPoppedHandler;
            Model.OnDisconnected -= OnPoppedHandler;
        }

        public void StartFlyDown()
        {
            trail.SetActive(true);
            StartCoroutine(FlyToGround());
        }

        private void OnPoppedHandler(Bubble bubble)
        {
            transform.SetParent(gameView, true);
            animator.SetBool("HasEscaped", true);

            if (celebration != null)
            {
                Instantiate(celebration, transform.position, Quaternion.identity);
            }

            Sound.PlaySoundEvent.Dispatch(escapeSound);
        }

        private void OnLevelComplete(LevelCompleteEvent gameEvent)
        {
            animator.SetBool("WonLevel", gameEvent.Won);
            animator.SetBool("LostLevel", !gameEvent.Won);

            if (!gameEvent.Won)
            {
                Sound.PlaySoundEvent.Dispatch(loseSound);
            }
        }

        private void OnLowMoves(LowMovesEvent gameEvent)
        {
            animator.SetBool("LosingLevel", true);
        }

        private IEnumerator FlyToGround()
        {
            var myTransform = transform;
            var speed = GlobalState.Instance.Config.woodstock.flightSpeed;
            var path = new Paths.WindingPath(myTransform.localPosition, LandingSpot);

            do
            {
                myTransform.localPosition = path.Advance(speed * Time.deltaTime);
                yield return null;
            } while (!path.Complete);

            animator.SetBool("OnGround", true);
            trail.SetActive(false);
        }
    }
}
