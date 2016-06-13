using System.Collections.Generic;
using System.Linq;

public class Bubble
{
    public delegate void BubbleHandler();

    public event BubbleHandler OnPopped;
    public event BubbleHandler OnDisconnected;

    public bool IsRoot { get; set; }
    public BubbleType type;
    public BubbleDefinition definition;
    public List<Bubble> connections = new List<Bubble>();

    public void AddConnection(Bubble bubble)
    {
        if (!connections.Contains(bubble))
        {
            connections.Add(bubble);
            bubble.AddConnection(this);
        }
    }

    public void RemoveConnection(Bubble bubble)
    {
        if (connections.Contains(bubble))
        {
            connections.Remove(bubble);
            bubble.RemoveConnection(this);
        }
    }

    public void RemoveAllConnections()
    {
        while (connections.Count > 0)
        {
            RemoveConnection(connections[0]);
        }
    }

    public void CheckForMatches()
    {
        var bubbleList = new List<Bubble>();
        var detachList = new List<Bubble>();

        CheckForMatches(bubbleList, detachList);

        if (bubbleList.Count >= 3)
        {
            foreach (var bubble in bubbleList)
            {
                detachList.Remove(bubble);
                bubble.RemoveAllConnections();
                BubbleReactionEvent.Dispatch(ReactionPriority.Pop, bubble.PopBubble);
            }

            CheckForFallingBubbles(detachList);
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
        RemoveAllConnections();

        if (OnDisconnected != null)
        {
            OnDisconnected();
        }
    }

    private void CheckForMatches(List<Bubble> bubbleList, List<Bubble> detachList)
    {
        foreach (var connection in connections)
        {
            if ((connection.type == type) && !bubbleList.Contains(connection))
            {
                bubbleList.Add(connection);
                connection.CheckForMatches(bubbleList, detachList);

                foreach (var neighbor in connection.connections)
                {
                    if (!detachList.Contains(neighbor))
                    {
                        detachList.Add(neighbor);
                    }
                }
            }
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

            foreach (var neighbor in bubble.connections)
            {
                ScanForRoots(neighbor, connected);
            }
        }
    }

    private static void PropogateRoot(Dictionary<Bubble, bool> connected, Bubble root)
    {
        foreach (var neighbor in root.connections)
        {
            if (!connected[neighbor])
            {
                connected[neighbor] = true;
                PropogateRoot(connected, neighbor);
            }
        }
    }
}
