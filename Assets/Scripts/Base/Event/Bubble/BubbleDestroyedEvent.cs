using UnityEngine;
using System.Collections;

public class BubbleDestroyedEvent : GameEvent
{
    public int score;

    public BubbleDestroyedEvent(int bubbleScore)
    {
        score = bubbleScore;
    }
}
