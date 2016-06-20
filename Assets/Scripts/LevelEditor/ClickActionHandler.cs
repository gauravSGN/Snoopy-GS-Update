using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace LevelEditor
{
    public class ClickActionHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private struct GridPosition
        {
            public int X;
            public int Y;
        }

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private RectTransform bubbleContainer;

        private RectTransform rectTransform;
        private readonly List<GridPosition> modifiedPositions = new List<GridPosition>();
        private bool dragging;

        protected void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            dragging = true;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (dragging)
            {
                ApplyAction(eventData);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            dragging = false;
            modifiedPositions.Clear();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!dragging)
            {
                ApplyAction(eventData);
            }
        }

        private void ApplyAction(PointerEventData eventData)
        {
            var gridCoord = GetGridCoordinate(eventData);

            if (IsValidCoordinate(gridCoord))
            {
                if (dragging)
                {
                    if (modifiedPositions.Contains(gridCoord))
                    {
                        return;
                    }

                    modifiedPositions.Add(gridCoord);
                }

                manipulator.PerformAction(gridCoord.X, gridCoord.Y);
            }
        }

        private GridPosition GetGridCoordinate(PointerEventData eventData)
        {
            var clickPosition = GetClickPosition(eventData);
            var boardX = clickPosition.x / 32.0f;
            var boardY = (clickPosition.y - bubbleContainer.localPosition.y) / 32.0f / MathUtil.COS_30_DEGREES;

            float maxDist = 999.0f;
            Vector2 nearest = Vector2.zero;

            foreach (var bubblePosition in GetNearbyPositions((int)(boardX - 0.5f * ((int)boardY & 1)), (int)boardY))
            {
                var dist2 = (bubblePosition.x - boardX) * (bubblePosition.x - boardX) + (bubblePosition.y - boardY) * (bubblePosition.y - boardY);

                if (dist2 < maxDist)
                {
                    maxDist = dist2;
                    nearest = bubblePosition;
                }
            }

            var bubbleY = (int)Mathf.Round(nearest.y - 0.5f);
            var bubbleX = (int)Mathf.Round(nearest.x - 0.5f - 0.5f * (bubbleY & 1));

            return new GridPosition { X = bubbleX, Y = bubbleY };
        }

        private Vector2 GetClickPosition(PointerEventData eventData)
        {
            return new Vector2(
                Input.mousePosition.x - rectTransform.position.x - rectTransform.rect.x,
                rectTransform.rect.height - (Input.mousePosition.y - rectTransform.position.y - rectTransform.rect.y)
            );
        }

        private IEnumerable<Vector2> GetNearbyPositions(int boardX, int boardY)
        {
            float baseX = boardX + 0.5f + (0.5f * (boardY & 1));
            float baseY = boardY + 0.5f;

            yield return new Vector2(baseX, baseY);
            yield return new Vector2(baseX - 0.5f, baseY - 1.0f);
            yield return new Vector2(baseX + 0.5f, baseY - 1.0f);
            yield return new Vector2(baseX - 1.0f, baseY);
            yield return new Vector2(baseX + 1.0f, baseY);
            yield return new Vector2(baseX - 0.5f, baseY + 1.0f);
            yield return new Vector2(baseX + 0.5f, baseY + 1.0f);
        }

        private bool IsValidCoordinate(GridPosition coord)
        {
            int bubblesPerRow = 12 - (coord.Y & 1);
            return (coord.X >= 0) && (coord.X < bubblesPerRow) && (coord.Y >= 0);
        }
    }
}
