using Graph;
using Reaction;
using System;
using System.Collections.Generic;

[Serializable]
public class Bubble : GraphElement<Bubble>
{
    public event Action<Bubble> OnSnap;
    public event Action<Bubble> OnPopped;
    public event Action<Bubble> OnDisconnected;
    public event Action<Bubble, bool> OnActivationChanged;

    public BubbleType type;
    public BubbleDefinition definition;

    private bool active = false;

    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            var previousValue = active;
            active = definition.Activatible && value;

            if ((active != previousValue) && (OnActivationChanged != null))
            {
                OnActivationChanged(this, active);
            }
        }
    }

    override public void RemoveFromGraph()
    {
        base.RemoveFromGraph();

        BubbleReactionEvent.Dispatch(ReactionPriority.Cull, this);
    }

    public bool CheckForMatches()
    {
        var bubbleList = new List<Bubble>();
        bubbleList.Add(this);

        foreach (Bubble bubble in Neighbors)
        {
            if (!bubbleList.Contains(bubble) && IsMatching(bubble))
            {
                bubbleList.Add(bubble);
                GraphUtil.MatchNeighbors(bubbleList, bubble, bubble.IsMatching);
            }
        }

        var matchMade = bubbleList.Count >= definition.MatchThreshold;

        if (matchMade)
        {
            foreach (var bubble in bubbleList)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.Pop, bubble);
            }
        }

        return matchMade;
    }

    public bool IsMatching(Bubble bubble)
    {
        return (bubble.definition.Color & definition.Color) > 0 && bubble.Active;
    }

    public void PopBubble()
    {
        if (OnPopped != null)
        {
            OnPopped(this);
        }
    }

    public void SnapToBubble()
    {
        if (OnSnap != null)
        {
            OnSnap(this);
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
