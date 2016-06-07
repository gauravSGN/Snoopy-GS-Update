using UnityEngine;
using System.Collections;

public class BubbleDestroyedEvent : IGameEvent
{
    public int score;
    public GameObject bubble;

    public BubbleDestroyedEvent(int bubbleScore, GameObject destroyedBubble)
    {
        score = bubbleScore;
        bubble = destroyedBubble;
    }
}
