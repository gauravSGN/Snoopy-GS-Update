using System;
using Graph;

[Serializable]
public class Bubble : GraphElement<Bubble>
{
    public event Action<Bubble> OnPopped;
    public event Action<Bubble> OnDisconnected;

    public BubbleType type;
    public BubbleDefinition definition;

    override public void RemoveFromGraph()
    {
        base.RemoveFromGraph();

        BubbleReactionEvent.Dispatch(ReactionPriority.Cull, MakeBubbleFall);
    }

    public void CheckForMatches()
    {
        var bubbleList = GraphUtil.MatchNeighbors(this, b => b.type == type);

        if (bubbleList.Count >= 3)
        {
            GraphUtil.RemoveNodes(bubbleList);

            foreach (var bubble in bubbleList)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.Pop, bubble.PopBubble);
            }
        }
    }

    public void PopBubble()
    {
        if (OnPopped != null)
        {
            OnPopped(this);
        }
    }

    public void MakeBubbleFall()
    {
        DisconnectAll();

        if (OnDisconnected != null)
        {
            OnDisconnected(this);
        }
    }
}
