using UnityEngine;
using System.Collections.Generic;
using Graph;
using Reaction;

public class ChainPopAdjacent : MonoBehaviour
{
    protected void Start()
    {
        GetComponent<BubbleModelBehaviour>().Model.OnSnap += OnSnapHandler;
    }

    protected void OnDestroy()
    {
        GetComponent<BubbleModelBehaviour>().Model.OnSnap -= OnSnapHandler;
    }

    private void OnSnapHandler(Bubble bubble)
    {
        BubbleReactionEvent.Dispatch(ReactionPriority.ChainPop, bubble);
    }
}
