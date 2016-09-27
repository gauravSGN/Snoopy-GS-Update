using UnityEngine;
using Effects;

public class BubbleSnap : SnapToGrid
{
    private Rigidbody2D rigidBody;
    private new CircleCollider2D collider;
    private BubbleAttachments attachments;

    public void CompleteSnap()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = 1.0f;
        rigidBody.isKinematic = true;

        collider.radius /= GlobalState.Instance.Config.bubbles.shotColliderScale;
        gameObject.layer = (int)Layers.GameObjects;

        GlobalState.EventService.Dispatch(new BubbleSettlingEvent());
        Destroy(this);

        if (!attachments.Model.CheckForMatches())
        {
            Sound.PlaySoundEvent.Dispatch(attachments.Model.definition.Sounds.impact);
        }

        GlobalState.EventService.Dispatch(new BubbleSettledEvent { shooter = gameObject });
    }

    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        attachments = GetComponent<BubbleAttachments>();

        collider.radius *= GlobalState.Instance.Config.bubbles.shotColliderScale;
        attachments.Model.Active = true;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == StringConstants.Tags.BUBBLES)
        {
            SnapIntoPlace();
        }
        else
        {
            var config = GlobalState.Instance.Config;
            var gameObjectLayer = 1 << (int)Layers.GameObjects;
            var velocity = (Vector3)rigidBody.velocity.normalized;
            var position = transform.position;
            var radius = config.bubbles.size / 3.0f;

            velocity = new Vector3(Mathf.Abs(velocity.x) * Mathf.Sign(position.x), velocity.y);
            position -= velocity * radius;

            var otherCollider = Physics2D.OverlapCircle(position, radius, gameObjectLayer);

            if (otherCollider != null)
            {
                transform.position = position - velocity * radius;
                SnapIntoPlace();
            }
            else
            {
                Sound.PlaySoundEvent.Dispatch(attachments.Model.definition.Sounds.bounce);
            }
        }
    }

    private void SnapIntoPlace()
    {
        AdjustToGrid();
        var origin = transform.position;

        foreach (var bubble in NearbyBubbles(origin))
        {
            AttachToBubble(bubble.gameObject);
            bubble.GetComponent<BubbleAttachments>().Model.SnapToBubble();
        }

        foreach (var bubble in NearbyBubbles(origin, GlobalState.Instance.Config.impactEffect.radius))
        {
            var bubbleEffectController = bubble.GetComponent<BubbleEffectController>();

            if (bubbleEffectController != null)
            {
                bubbleEffectController.AddEffect(ImpactShockwaveEffect.Play(bubble.gameObject, origin));
            }
        }

        CompleteSnap();
    }

    private void AttachToBubble(GameObject bubble)
    {
        attachments.Attach(bubble);
        attachments.Model.MinimizeDistanceFromRoot();
        attachments.Model.SortNeighbors();
    }
}
