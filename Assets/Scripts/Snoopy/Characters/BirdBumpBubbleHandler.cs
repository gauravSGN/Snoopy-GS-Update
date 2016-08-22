using UnityEngine;
using System.Collections;
using System.Collections.Generic;

sealed public class BirdBumpBubbleHandler : MonoBehaviour
{
    private Transform parentBubble;

    public void BirdBumpBubble()
    {
        parentBubble = parentBubble ?? transform.parent.parent;

        StartCoroutine(DoBumpEffect());
    }

    private IEnumerator DoBumpEffect()
    {
        var originalScale = parentBubble.localScale;
        var config = GlobalState.Instance.Config.woodstock;

        parentBubble.localScale *= config.bumpScale;
        yield return new WaitForSeconds(config.bumpDuration);

        parentBubble.localScale = originalScale;
    }
}
