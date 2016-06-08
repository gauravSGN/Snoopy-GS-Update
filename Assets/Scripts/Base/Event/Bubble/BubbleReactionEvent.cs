using UnityEngine;
using System;

public class BubbleReactionEvent : GameEvent
{
    public ReactionPriority priority;
    public Action action;
}
