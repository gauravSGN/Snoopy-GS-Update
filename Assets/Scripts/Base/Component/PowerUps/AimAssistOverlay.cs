using Util;
using Aiming;
using UnityEngine;
using System.Collections.Generic;

namespace PowerUps
{
    sealed public class AimAssistOverlay : MonoBehaviour
    {
        [SerializeField]
        private GameObject overlayPrefab;

        [SerializeField]
        private PowerUpScanMap scanMap;

        [SerializeField]
        private SnapToGrid snapToGrid;

        private bool assistActive = false;
        private GameObjectPool overlayPool = new GameObjectPool();
        private List<SpriteRenderer> overlays = new List<SpriteRenderer>();


        public void Start()
        {
            GlobalState.EventService.AddEventHandler<PowerUpAppliedEvent>(OnApplied);

            overlayPool.Allocate(overlayPrefab, 7);
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
            Clear();
        }

        private void Activate()
        {
            EnableOverlays(true);
        }

        private void Deactivate()
        {
            // Important in case the shape gets resized and collides with a wall turning on some overlays
            transform.position = new Vector3(-1000, -1000, 0);
            EnableOverlays(false);
        }

        private void OnPosition(AimPositionEvent gameEvent)
        {
            transform.position = gameEvent.position;
            snapToGrid.AdjustToGrid();
        }

        private void Clear()
        {
            foreach (var overlay in overlays)
            {
                overlayPool.Release(overlay.gameObject);
            }

            overlays.Clear();
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
            Clear();

            var shape = GetShapeData(type);
            var bubbleSize = GlobalState.Instance.Config.bubbles.size;
            var yBubbleSize = bubbleSize * MathUtil.COS_30_DEGREES;
            var basePosition = transform.position;

            foreach (var locations in shape)
            {
                foreach (var location in locations)
                {
                    var origin = new Vector2(basePosition.x + (location.x * bubbleSize),
                                             basePosition.y + (location.y * yBubbleSize));

                    PositionOverlay(origin);
                }
            }
        }

        private Vector2[][] GetShapeData(PowerUpType type)
        {
            Vector2[][] shape;

            if (type == PowerUpType.ThreeCombo)
            {
                shape = GetBigCombo(PowerUpController.THREE_COMBO_ROWS);
            }
            else if (type == PowerUpType.FourCombo)
            {
                shape = GetBigCombo(PowerUpController.FOUR_COMBO_ROWS);
            }
            else
            {
                shape = scanMap.Map[type].locations;
            }

            return shape;
        }

        private Vector2[][] GetBigCombo(int rows)
        {
            var locations = new List<Vector2>();
            var numPerRow = GlobalState.Instance.Config.bubbles.numPerRow;

            for (int row = -rows; row <= rows; row++)
            {
                for (int column = -numPerRow; column < numPerRow; column++)
                {
                    locations.Add(new Vector2(column - (0.5f * (row % 2)), row));
                }
            }

            var returnArray = new Vector2[1][];
            returnArray[0] = locations.ToArray();

            return returnArray;
        }

        private void PositionOverlay(Vector3 position)
        {
            GameObject overlay = overlayPool.Get(overlayPrefab);
            overlay.transform.parent = transform;
            overlay.transform.position = position;

            var renderer = overlay.GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            overlays.Add(renderer);
        }
    }
}
