using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    public class MapScroller : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private RectTransform scrollingContent;

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<SnapMapToLocationEvent>(OnSnapMapToLocation);
        }

        public void OnSnapMapToLocation(SnapMapToLocationEvent gameEvent)
        {
            Canvas.ForceUpdateCanvases();

            var scrollWidth = scrollRect.GetComponent<RectTransform>().rect.width;
            var targetPosition = (Vector2)scrollRect.transform.InverseTransformPoint(gameEvent.target.position);

            var targetX = (targetPosition.x - (scrollWidth / 2));
            var clampedX = Math.Min(Math.Max(targetX, 0), (scrollingContent.rect.width - scrollWidth));

            scrollingContent.localPosition = new Vector2(-clampedX, 0);
        }
    }
}