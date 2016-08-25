using UnityEngine;
using System.Collections.Generic;

public class BreakOnCull : BubbleModelBehaviour
{
    override protected void AddListeners()
    {
        Model.OnDisconnected += OnDisconnected;
    }

    override protected void RemoveListeners()
    {
        Model.OnDisconnected -= OnDisconnected;
    }

    private void OnDisconnected(Bubble bubble)
    {
        RemoveListeners();

        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Cull);
    }
}
