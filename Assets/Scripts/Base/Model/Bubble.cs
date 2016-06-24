using System;
using Graph;
using Reaction;

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

        BubbleReactionEvent.Dispatch(ReactionPriority.Cull, this);
    }

    public void CheckForMatches()
    {
        var bubbleList = GraphUtil.MatchNeighbors(this, b => b.type == type);

        if (bubbleList.Count >= 3)
        {
            foreach (var bubble in bubbleList)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.Pop, bubble);
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
