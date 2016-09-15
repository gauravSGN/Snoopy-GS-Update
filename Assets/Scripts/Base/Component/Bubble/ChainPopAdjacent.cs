using UnityEngine;
using System.Collections.Generic;
using Graph;
using Reaction;

public class ChainPopAdjacent : MonoBehaviour
{
    protected void Start()
    {
        var model = GetComponent<BubbleModelBehaviour>().Model;
        model.OnSnap += PopAdjacent;
        model.OnPopped += PopAdjacent;
    }

    protected void OnDestroy()
    {
        var model = GetComponent<BubbleModelBehaviour>().Model;
        model.OnSnap -= PopAdjacent;
        model.OnPopped -= PopAdjacent;
    }

    private void PopAdjacent(Bubble bubble)
    {
        BubbleReactionEvent.Dispatch(ReactionPriority.ChainPop, bubble);
    }
}
