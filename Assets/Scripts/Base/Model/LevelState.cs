using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelState : Observable<LevelState>
{
    public int score;
    public int remainingBubbles;
    public Dictionary<BubbleType, int> typeTotals = new Dictionary<BubbleType, int>();
    public BubbleQueue bubbleQueue = new BubbleQueue();
}
