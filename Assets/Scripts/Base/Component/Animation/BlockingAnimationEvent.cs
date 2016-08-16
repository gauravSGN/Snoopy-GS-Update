using UnityEngine;
using System;

public class BlockingAnimationEvent : MonoBehaviour
{
    public event Action StopBlocking;

    public void FinishBlocking()
    {
        if (StopBlocking != null)
        {
            StopBlocking();
        }
    }
}
