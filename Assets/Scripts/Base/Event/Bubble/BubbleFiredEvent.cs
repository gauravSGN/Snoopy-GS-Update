using UnityEngine;
using System.Collections;

public class BubbleFiredEvent : GameEvent
{
    public GameObject bubble;

    public BubbleFiredEvent(GameObject firedBubble)
    {
        bubble = firedBubble;
    }
}
