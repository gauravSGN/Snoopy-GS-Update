using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Snoopy.Characters
{
    sealed public class BirdBumpBubbleHandler : MonoBehaviour
    {
        private Transform parentBubble;
        private Vector3 originalScale;

        public void Start()
        {
            parentBubble = transform.parent.parent;
            originalScale = parentBubble.localScale;
        }

        public void BirdBumpBubble()
        {
            StartCoroutine(DoBumpEffect());
        }

        private IEnumerator DoBumpEffect()
        {
            var config = GlobalState.Instance.Config.woodstock;

            parentBubble.localScale = originalScale * config.bumpScale;
            yield return new WaitForSeconds(config.bumpDuration);

            parentBubble.localScale = originalScale;
        }
    }
}
