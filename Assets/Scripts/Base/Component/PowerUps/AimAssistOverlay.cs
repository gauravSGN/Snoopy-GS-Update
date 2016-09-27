using Util;
using Aiming;
using UnityEngine;
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

        [SerializeField]
        private SnapToGrid snapToGrid;

        private List<SpriteRenderer> overlays = new List<SpriteRenderer>();
        private List<GameObject> positioned = new List<GameObject>();
        private List<GameObject> waitingForPosition = new List<GameObject>();
        private bool assistActive = false;

        public void Start()
        {
            GlobalState.EventService.AddEventHandler<PowerUpAppliedEvent>(OnApplied);
        }

        private void Activate()
        {
            EnableOverlays(true);
        }

        private void Deactivate()
        {
            EnableOverlays(false);
        }

        private void OnApplied(PowerUpAppliedEvent gameEvent)
        {
            if (!assistActive)
            {
                var eventService = GlobalState.EventService;
                eventService.AddEventHandler<BubbleSettledEvent>(OnSettled);
                eventService.AddEventHandler<StartAimingEvent>(Activate);
                eventService.AddEventHandler<StopAimingEvent>(Deactivate);
                eventService.AddEventHandler<AimPositionEvent>(OnPosition);

                assistActive = true;
            }

            SetShape(gameEvent.type);
        }

        private void OnSettled()
        {
            assistActive = false;

            var eventService = GlobalState.EventService;
            eventService.RemoveEventHandler<BubbleSettledEvent>(OnSettled);
            eventService.RemoveEventHandler<StartAimingEvent>(Activate);
            eventService.RemoveEventHandler<StopAimingEvent>(Deactivate);
            eventService.RemoveEventHandler<AimPositionEvent>(OnPosition);

            Deactivate();
        }

        private void OnPosition(AimPositionEvent gameEvent)
        {
            transform.position = gameEvent.position;
            snapToGrid.Snap();
            transform.position += Vector3.back;
        }

        private void EnableOverlays(bool enabled)
        {
            foreach (var overlay in overlays)
            {
                overlay.enabled = enabled;
            }
        }

        private void SetShape(PowerUpType type)
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

        private void PositionOverlay(Vector3 position)
        {
            GameObject overlay = GetOverlay();

            overlay.transform.position = position;
            positioned.Add(overlay);

            var renderer = overlay.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            overlays.Add(renderer);
        }

        private GameObject GetOverlay()
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

            return overlay;
        }
    }
}
