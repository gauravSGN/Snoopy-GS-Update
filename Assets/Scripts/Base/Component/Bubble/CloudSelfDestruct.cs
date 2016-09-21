using Graph;
using UnityEngine;
using System.Collections.Generic;
using Reaction;

sealed public class CloudSelfDestruct : MonoBehaviour
{
    private bool scanScheduled = false;

    public void Start()
    {
        GetComponent<BubbleModelBehaviour>().Model.OnRemoveNeighbor += OnRemoveNeighbor;
    }

    public void OnDestroy()
    {
        GetComponent<BubbleModelBehaviour>().Model.OnRemoveNeighbor -= OnRemoveNeighbor;
    }

    private void OnRemoveNeighbor(Bubble neighbor)
    {
        if (!scanScheduled)
        {
            Util.FrameUtil.OnNextFrame(PerformScanForNonClouds);
            scanScheduled = true;
        }
    }

    private void PerformScanForNonClouds()
    {
        var model = GetComponent<BubbleModelBehaviour>().Model;
        var attached = GraphUtil.SearchBreadthFirst(model, false, AttachedToNonCloud);

        if (!attached)
        {
            BubbleReactionEvent.Dispatch(ReactionPriority.ChainPop, model);
        }

        scanScheduled = false;
    }

    private bool AttachedToNonCloud(Bubble bubble, ref bool value)
    {
        foreach (Bubble neighbor in bubble.Neighbors)
        {
            if (neighbor.definition.Type != BubbleType.Cloud)
            {
                value = true;
                return false;
            }
        }

        return true;
    }
}
