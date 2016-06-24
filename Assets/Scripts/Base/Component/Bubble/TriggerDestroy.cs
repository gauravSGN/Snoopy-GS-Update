using UnityEngine;
using Reaction;

public class TriggerDestroy : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == StringConstants.Tags.BUBBLES) && collider.gameObject.GetComponent<BubbleSnap>())
        {
            var model = gameObject.GetComponent<BubbleAttachments>().Model;
            BubbleReactionEvent.Dispatch(ReactionPriority.TriggerDestroy, model);
        }
    }
}
