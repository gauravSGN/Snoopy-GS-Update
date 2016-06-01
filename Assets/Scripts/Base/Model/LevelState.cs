﻿using UnityEngine;
using System;
using System.Collections;

public class LevelState : Observable<LevelState>
{
    public int remainingBubbles;
    public BubbleQueue bubbleQueue = new BubbleQueue();
}