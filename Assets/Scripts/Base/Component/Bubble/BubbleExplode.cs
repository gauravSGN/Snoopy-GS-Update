using UnityEngine;
using System.Collections;

public class BubbleExplode : MonoBehaviour
{
    private const float BUBBLE_SPACING = 0.225f;

    public int sizeMultiplier = 2;

    public void Setup(int explosionSize)
    {
        sizeMultiplier = explosionSize;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bubble")
        {
            Debug.Log("Am I getting fired");
            BubbleReactionEvent.Dispatch(ReactionPriority.Explode, this.Explode);
        }
    }

    public void Explode()
    {
        Debug.Log(sizeMultiplier.ToString());
        foreach (var hit in Physics2D.CircleCastAll(transform.position, BUBBLE_SPACING * sizeMultiplier, Vector2.up, 0.0f))
        {
            if (hit.collider.gameObject.tag == "Bubble")
            {
                hit.collider.gameObject.transform.position = new Vector3(-1000.0f, -1000.0f);
                var attachment = hit.collider.gameObject.GetComponent<BubbleAttachments>();
                attachment.Model.RemoveAllConnections();
                attachment.MarkForDestruction();
            }
        }
    }
}
