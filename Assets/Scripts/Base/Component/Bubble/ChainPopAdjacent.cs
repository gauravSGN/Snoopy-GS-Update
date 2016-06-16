using UnityEngine;
using System.Collections.Generic;
using Graph;

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

    private void PoppedHandler()
    {
        var model = GetComponent<BubbleAttachments>().Model;
        var adjacentList = GetAdjacentBubbles(new List<Bubble>(), model);

        adjacentList.Remove(model);
        GraphUtil.RemoveNodes(adjacentList);
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
