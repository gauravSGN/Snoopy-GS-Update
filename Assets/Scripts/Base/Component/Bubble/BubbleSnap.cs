using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Effects;

public class BubbleSnap : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private new CircleCollider2D collider;
    private BubbleAttachments attachments;

    public void CompleteSnap()
    {
        if (enabled)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 1.0f;
            rigidBody.isKinematic = true;

            collider.radius /= GlobalState.Instance.Config.bubbles.shotColliderScale;
            gameObject.layer = (int)Layers.GameObjects;

            GlobalState.EventService.Dispatch(new BubbleSettlingEvent());
            Destroy(this);
            enabled = false;

            if (!attachments.Model.CheckForMatches())
            {
                Sound.PlaySoundEvent.Dispatch(attachments.Model.definition.Sounds.impact);
            }

            GlobalState.EventService.Dispatch(new BubbleSettledEvent { shooter = gameObject });
        }
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

    private void AdjustToGrid()
    {
        var myPosition = (Vector2)transform.position;
        var nearbyBubbles = NearbyBubbles(transform.position).Select(b => b.gameObject).ToArray();
        var attachPoints = GetAttachmentPoints(nearbyBubbles).OrderBy(p => (p - myPosition).sqrMagnitude).ToArray();

        foreach (var attachPoint in attachPoints)
        {
            if (CanPlaceAtLocation(attachPoint))
            {
                transform.position = attachPoint;
                return;
            }
        }
    }

    private void AttachToBubble(GameObject bubble)
    {
        attachments.Attach(bubble);
        attachments.Model.MinimizeDistanceFromRoot();
        attachments.Model.SortNeighbors();
    }

    private IEnumerable<Collider2D> NearbyBubbles(Vector2 location)
    {
        return NearbyBubbles(location, GlobalState.Instance.Config.bubbles.size);
    }

    private IEnumerable<Collider2D> NearbyBubbles(Vector2 location, float radius)
    {
        foreach (var hit in Physics2D.CircleCastAll(location, radius, Vector2.up, 0.0f))
        {
            if ((hit.collider.gameObject != gameObject) &&
                (hit.collider.gameObject.tag == StringConstants.Tags.BUBBLES))
            {
                yield return hit.collider;
            }
        }
    }

    private bool CanPlaceAtLocation(Vector2 location)
    {
        var halfSize = GlobalState.Instance.Config.bubbles.size / 2.0f;

        foreach (var hit in Physics2D.CircleCastAll(location, halfSize * 0.9f, Vector2.up, 0.0f,
                                                    (1 << (int)Layers.GameObjects | 1 << (int)Layers.Walls)))
        {
            if (hit.collider.gameObject != gameObject)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerable<Vector2> GetAttachmentPoints(IEnumerable<GameObject> bubbles)
    {
        var theta = Mathf.PI / 3.0f;
        var bubbleSize = GlobalState.Instance.Config.bubbles.size;

        foreach (var bubble in bubbles)
        {
            var bubblePosition = (Vector2)bubble.transform.position;

            for (var index = 0; index < 6; index++)
            {
                yield return new Vector2(
                    bubblePosition.x + Mathf.Cos(index * theta) * bubbleSize,
                    bubblePosition.y + Mathf.Sin(index * theta) * bubbleSize
                );
            }
        }
    }
}
