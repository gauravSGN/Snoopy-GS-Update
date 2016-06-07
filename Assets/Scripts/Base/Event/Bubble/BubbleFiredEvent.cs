using UnityEngine;
using System.Collections;

public class BubbleFiredEvent : IGameEvent
{
    public GameObject bubble;

    public BubbleFiredEvent(GameObject firedBubble)
    {
        bubble = firedBubble;
    }
}
