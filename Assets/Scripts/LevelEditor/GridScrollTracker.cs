using UnityEngine;

namespace LevelEditor
{
    public class GridScrollTracker : MonoBehaviour
    {
        protected float ContentPosition { get { return content.localPosition.y; } }

        [SerializeField]
        protected RectTransform content;

        [SerializeField]
        protected float wrapHeight;

        protected RectTransform rectTransform;
        protected float lastY;
        private float deltaY;

        virtual protected void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            lastY = ContentPosition;
        }

        virtual protected void LateUpdate()
        {
            if (Mathf.Abs(ContentPosition - lastY) > Mathf.Epsilon)
            {
                lastY = ContentPosition;
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            var previousDelta = deltaY;
            deltaY = ((lastY % wrapHeight) + wrapHeight) % wrapHeight;

            var oldPosition = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(
                oldPosition.x,
                oldPosition.y - previousDelta + deltaY,
                oldPosition.z
            );
        }
    }
}
