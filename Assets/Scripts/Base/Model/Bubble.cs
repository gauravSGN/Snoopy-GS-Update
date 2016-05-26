using UnityEngine;
using System.Collections.Generic;

public class Bubble
{
    public delegate void BubbleHandler();

    public event BubbleHandler OnPopped;

    public BubbleType type;
    public List<Bubble> connections = new List<Bubble>();

    public void AddConnection(Bubble bubble)
    {
        if (!connections.Contains(bubble))
        {
            connections.Add(bubble);
            bubble.AddConnection(this);
        }
    }

    public void CheckForMatches()
    {
        var bubbleList = new List<Bubble>();
        CheckForMatches(bubbleList);

        if (bubbleList.Count >= 3)
        {
            foreach (var bubble in bubbleList)
            {
                if (bubble.OnPopped != null)
                {
                    bubble.OnPopped();
                }
            }
        }
    }

    private void CheckForMatches(List<Bubble> bubbleList)
    {
        foreach (var connection in connections)
        {
            if ((connection.type == type) && !bubbleList.Contains(connection))
            {
                bubbleList.Add(connection);
                connection.CheckForMatches(bubbleList);
            }
        }
    }
}
