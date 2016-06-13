using UnityEngine;

public class BubbleDestroyedEvent : GameEvent
{
    public int score;
    public GameObject bubble;

    public BubbleDestroyedEvent(int bubbleScore, GameObject destroyedBubble)
    {
        score = bubbleScore;
        bubble = destroyedBubble;
    }
}
