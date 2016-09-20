using System;
using UnityEngine;

namespace GameTween
{
    public class TranslateTween : MonoBehaviour
    {
        [SerializeField]
        private float xOffset;

        [SerializeField]
        private float yOffset;

        [SerializeField]
        private float duration;

        [SerializeField]
        private AnimationCurve easeTo = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField]
        private AnimationCurve easeFrom = AnimationCurve.Linear(0, 0, 1, 1);

        // Overloads with no args are so Unity button callbacks can show these functions in editor
        public void PlayTo()
        {
            PlayTo(null);
        }

        public void PlayTo(Action<AbstractGoTween> onComplete)
        {
            PlayTween(xOffset, yOffset, TweenMethod.PositionTo, onComplete);
        }

        public void PlayToBack()
        {
            PlayToBack(null);
        }

        public void PlayToBack(Action<AbstractGoTween> onComplete)
        {
            PlayTween(-xOffset, -yOffset, TweenMethod.PositionTo, onComplete);
        }

        public void PlayFrom()
        {
            PlayFrom(null);
        }

        public void PlayFrom(Action<AbstractGoTween> onComplete)
        {
            PlayTween(-xOffset, -yOffset, TweenMethod.PositionFrom, onComplete);
        }

        public void PlayFromBack()
        {
            PlayFromBack(null);
        }

        public void PlayFromBack(Action<AbstractGoTween> onComplete)
        {
            PlayTween(xOffset, yOffset, TweenMethod.PositionFrom, onComplete);
        }

        private void PlayTween(float xOffset, float yOffset, TweenMethod type, Action<AbstractGoTween> onComplete = null)
        {
            GoTween tween;
            var vec = transform.localToWorldMatrix.MultiplyVector(new Vector3(xOffset, yOffset, 0));

            if (type == TweenMethod.PositionFrom)
            {
                tween = transform.positionFrom(duration, vec, true);
                tween.easeCurve = easeFrom;
            }
            else
            {
                tween = transform.positionTo(duration, vec, true);
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