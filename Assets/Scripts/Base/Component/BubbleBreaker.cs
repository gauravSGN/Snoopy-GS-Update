using UnityEngine;

public class BubbleBreaker : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Cull);
    }
}
