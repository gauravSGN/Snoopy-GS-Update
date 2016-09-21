using Util;
using Aiming;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace PowerUps
{
    sealed public class AimAssistOverlay : MonoBehaviour
    {
        [SerializeField]
        private AimLineEventTrigger eventTrigger;

        [SerializeField]
        private GameObject overlayPrefab;

        [SerializeField]
        private PowerUpScanMap scanMap;

        private List<SpriteRenderer> overlays = new List<SpriteRenderer>();
        private List<GameObject> positioned = new List<GameObject>();
        private List<GameObject> waitingForPosition = new List<GameObject>();

        public void Start()
        {
            SetShape(PowerUpType.Yellow);

            var eventService = GlobalState.EventService;
            eventService.AddEventHandler<StartAimingEvent>(Activate);
            eventService.AddEventHandler<StopAimingEvent>(Deactivate);
            eventService.AddEventHandler<BubbleSettledEvent>(Deactivate);

            eventTrigger.MoveTarget += OnMoveTarget;
        }

        public void SetShape(PowerUpType type)
        {
            Deactivate();
            var definition = scanMap.Map[type];


            waitingForPosition.AddRange(positioned);
            positioned = new List<GameObject>();
            overlays.Clear();

            var bubbleSize = GlobalState.Instance.Config.bubbles.size;
            var yBubbleSize = bubbleSize * MathUtil.COS_30_DEGREES;
            var basePosition = transform.position;

            foreach (var locations in definition.locations)
            {
                foreach (var location in locations)
                {
                    var origin = new Vector2(basePosition.x + (location.x * bubbleSize),
                                             basePosition.y + (location.y * yBubbleSize));

                    PositionOverlay(origin);
                }
            }
        }

        public void Activate()
        {
            Debug.Log("Activate");
            EnableOverlays(true);
        }

        public void Deactivate()
        {
            Debug.Log("Deactivate");
            EnableOverlays(false);
        }

        private void OnMoveTarget(Vector2 target)
        {
            transform.position = (Vector3)target;
            transform.position += Vector3.back;
        }

        private void EnableOverlays(bool enabled)
        {
            foreach (var overlay in overlays)
            {
                overlay.enabled = enabled;
            }
            // overlays.Select(overlay => overlay.enabled = enabled);
        }

        private void PositionOverlay(Vector3 position)
        {
            GameObject overlay;

            if (waitingForPosition.Count > 0)
            {
                overlay = waitingForPosition[0];
                waitingForPosition.RemoveAt(0);
            }
            else
            {
                overlay = Instantiate(overlayPrefab);
                overlay.transform.parent = transform;
            }

            overlay.transform.position = position;
            positioned.Add(overlay);

            var renderer = overlay.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            overlays.Add(renderer);
        }
    }
}
