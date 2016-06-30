using System;
using System.Collections.Generic;
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
        var bubbleList = new List<Bubble>();
        bubbleList.Add(this);

        foreach (Bubble bubble in Neighbors)
        {
            if (!bubbleList.Contains(bubble) && MatchesColor(bubble))
            {
                bubbleList.Add(bubble);
                GraphUtil.MatchNeighbors(bubbleList, bubble, bubble.MatchesColor);
            }
        }

        if (bubbleList.Count >= 3)
        {
            foreach (var bubble in bubbleList)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.Pop, bubble);
            }
        }
    }

    public bool MatchesColor(Bubble bubble)
    {
        return (bubble.definition.Color & definition.Color) > 0;
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
