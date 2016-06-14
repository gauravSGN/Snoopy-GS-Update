using System.Collections.Generic;
using System.Linq;
using System;
using Graph;

[Serializable]
public class Bubble : GraphElement<Bubble>
{
    public delegate void BubbleHandler();

    public event BubbleHandler OnPopped;
    public event BubbleHandler OnDisconnected;

    public BubbleType type;
    public BubbleDefinition definition;

    override public void Connect(Bubble node)
    {
        base.Connect(node);

        ResetDistanceFromRoot();
        MinimizeDistanceFromRoot();
        PropagateRootDistance();
    }

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
            var detachList = GraphUtil.RemoveNodes(bubbleList);

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
            OnPopped();
        }
    }

    public void MakeBubbleFall()
    {
        DisconnectAll();

        if (OnDisconnected != null)
        {
            OnDisconnected();
        }
    }

    private static void CheckForFallingBubbles(List<Bubble> detached)
    {
        var connected = new Dictionary<Bubble, bool>();

        foreach (var bubble in detached)
        {
            ScanForRoots(bubble, connected);
            foreach (var root in connected.Where(p => p.Value).ToList())
            {
                PropogateRoot(connected, root.Key);
            }
        }

        foreach (var pair in connected)
        {
            if (!pair.Value)
            {
                BubbleReactionEvent.Dispatch(ReactionPriority.Cull, pair.Key.MakeBubbleFall);
            }
        }
    }

    private static void ScanForRoots(Bubble bubble, Dictionary<Bubble, bool> connected)
    {
        if (!connected.ContainsKey(bubble))
        {
            connected.Add(bubble, bubble.IsRoot);

            foreach (var neighbor in bubble.neighbors)
            {
                ScanForRoots(neighbor, connected);
            }
        }
    }

    private static void PropogateRoot(Dictionary<Bubble, bool> connected, Bubble root)
    {
        foreach (var neighbor in root.neighbors)
        {
            if (!connected[neighbor])
            {
                connected[neighbor] = true;
                PropogateRoot(connected, neighbor);
            }
        }
    }
}
