using UnityEngine;
using System.Collections.Generic;
using Graph;
using Reaction;

public class ChainPopAdjacent : MonoBehaviour
{
    protected void Start()
    {
        var attachments = GetComponent<BubbleAttachments>();
        attachments.Model.OnSnap += OnSnapHandler;
    }

    protected void OnDestroy()
    {
        var attachments = GetComponent<BubbleAttachments>();
        attachments.Model.OnSnap -= OnSnapHandler;
    }

    private void OnSnapHandler(Bubble bubble)
    {
        BubbleReactionEvent.Dispatch(ReactionPriority.ChainPop, bubble);
    }
}
