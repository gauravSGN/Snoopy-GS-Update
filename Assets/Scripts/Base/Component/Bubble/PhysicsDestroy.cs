using UnityEngine;
using Reaction;

public class PhysicsDestroy : MonoBehaviour
{
    [SerializeField]
    private bool destroyOnTrigger;

    [SerializeField]
    private bool destroyOnCollision;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (destroyOnCollision)
        {
            DestroyOnReaction(collision.collider);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (destroyOnTrigger)
        {
            DestroyOnReaction(collider);
        }
    }

    private void DestroyOnReaction(Collider2D collider)
    {
        if ((collider.gameObject.tag == StringConstants.Tags.BUBBLES) && collider.gameObject.GetComponent<BubbleSnap>())
        {
            var model = gameObject.GetComponent<BubbleModelBehaviour>().Model;
            BubbleReactionEvent.Dispatch(ReactionPriority.PhysicsDestroy, model);
        }
    }
}
