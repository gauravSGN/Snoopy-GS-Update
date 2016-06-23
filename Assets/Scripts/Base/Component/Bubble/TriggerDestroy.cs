using UnityEngine;
using System.Collections.Generic;
using Graph;

public class TriggerDestroy : MonoBehaviour
{
    public void OnPop()
    {
        var model = gameObject.GetComponent<BubbleAttachments>().Model;
        GraphUtil.RemoveNodes(new List<Bubble>(){model});
        model.PopBubble();
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == StringConstants.Tags.BUBBLES) && collider.gameObject.GetComponent<BubbleSnap>())
        {
            BubbleReactionEvent.Dispatch(ReactionPriority.TriggerDestroy, OnPop);
        }
    }
}