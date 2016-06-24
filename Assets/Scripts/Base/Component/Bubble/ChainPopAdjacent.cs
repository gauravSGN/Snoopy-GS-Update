using UnityEngine;
using System.Collections.Generic;
using Graph;
using Reaction;

public class ChainPopAdjacent : MonoBehaviour
{
    protected void Start()
    {
        var attachments = GetComponent<BubbleAttachments>();
        attachments.Model.OnPopped += PoppedHandler;
    }

    protected void OnDestroy()
    {
        var attachments = GetComponent<BubbleAttachments>();
        attachments.Model.OnPopped -= PoppedHandler;
    }

    private void PoppedHandler(Bubble bubble)
    {
        var adjacentList = GetAdjacentBubbles(new List<Bubble>(), bubble);

        adjacentList.Remove(bubble);
        GraphUtil.RemoveNodes(adjacentList);

        for (int index = 0, length = adjacentList.Count; index < length; index++)
        {
            BubbleReactionEvent.Dispatch(ReactionPriority.ChainPop, adjacentList[index]);
        }
    }

    private List<Bubble> GetAdjacentBubbles(List<Bubble> adjacentList, Bubble current)
    {
        adjacentList.Add(current);

        foreach (Bubble neighbor in current.Neighbors)
        {
            if ((neighbor.type == current.type) && !adjacentList.Contains(neighbor))
            {
                GetAdjacentBubbles(adjacentList, neighbor);
            }
        }

        return adjacentList;
    }
}
