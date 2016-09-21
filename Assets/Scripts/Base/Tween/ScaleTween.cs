using UnityEngine;
using System;

namespace GameTween
{
    public class ScaleTween : MonoBehaviour
    {
        [SerializeField]
        private float xScale;

        [SerializeField]
        private float yScale;

        [SerializeField]
        private float duration;

        [SerializeField]
        private AnimationCurve easeTo = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField]
        private AnimationCurve easeFrom = AnimationCurve.Linear(0, 0, 1, 1);

        private Vector3 originalScale;
        private Vector3 newScale;

        void Start()
        {
            originalScale = transform.localScale;
            newScale = new Vector3(xScale, yScale, 1f);
        }

        public void ScaleTo()
        {
            ScaleTo(null);
        }

        public void ScaleTo(Action<AbstractGoTween> onComplete)
        {
            PlayTween(newScale, TweenMethod.PositionTo, onComplete);
        }

        public void UnScaleTo()
        {
            UnScaleTo(null);
        }

        public void UnScaleTo(Action<AbstractGoTween> onComplete)
        {
            PlayTween(originalScale, TweenMethod.PositionTo, onComplete);
        }

        public void ScaleFrom()
        {
            ScaleFrom(null);
        }

        public void ScaleFrom(Action<AbstractGoTween> onComplete)
        {
            PlayTween(newScale, TweenMethod.PositionFrom, onComplete);
        }

        public void UnscaleFrom()
        {
            UnscaleFrom(null);
        }

        public void UnscaleFrom(Action<AbstractGoTween> onComplete)
        {
            PlayTween(originalScale, TweenMethod.PositionFrom, onComplete);
        }

        private void PlayTween(Vector3 scaleVec, TweenMethod type, Action<AbstractGoTween> onComplete = null)
        {
            GoTween tween;
            if (type == TweenMethod.PositionFrom)
            {
                tween = transform.scaleFrom(duration, scaleVec);
                tween.easeCurve = easeFrom;
            }
            else
            {
                tween = transform.scaleTo(duration, scaleVec);
                tween.easeCurve = easeTo;
            }

            tween.easeType = GoEaseType.AnimationCurve;

            if (onComplete != null)
            {
                tween.setOnCompleteHandler(onComplete);
            }
        }
    }
}