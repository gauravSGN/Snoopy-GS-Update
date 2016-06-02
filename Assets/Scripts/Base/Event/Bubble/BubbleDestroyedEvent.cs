using UnityEngine;
using System.Collections;

public class BubbleDestroyedEvent : IGameEvent
{
    public int score;

    public BubbleDestroyedEvent(int bubbleScore)
    {
        score = bubbleScore;
    }
}
