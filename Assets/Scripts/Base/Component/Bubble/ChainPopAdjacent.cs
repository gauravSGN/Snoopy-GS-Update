using UnityEngine;
using System.Collections.Generic;
using Graph;
using Reaction;

public class ChainPopAdjacent : MonoBehaviour
{
    protected void Start()
    {
        GetComponent<BubbleAttachments>().Model.OnSnap += OnSnapHandler;
    }

    protected void OnDestroy()
    {
        GetComponent<BubbleAttachments>().Model.OnSnap -= OnSnapHandler;
    }

    private void OnSnapHandler(Bubble bubble)
    {
        BubbleReactionEvent.Dispatch(ReactionPriority.ChainPop, bubble);
    }
}
