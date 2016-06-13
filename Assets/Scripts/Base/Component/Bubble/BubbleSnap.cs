using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BubbleSnap : MonoBehaviour
{
    private const float BUBBLE_SPACING = 0.25f;
    private const float RADIUS_FACTOR = 0.5f;
    private const string ATTACHED_LAYER = "Game Objects";
    private const string TRAVELING_LAYER = "Default";

    private Rigidbody2D rigidBody;
    private new CircleCollider2D collider;

    protected void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(TRAVELING_LAYER);
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();

        collider.radius *= RADIUS_FACTOR;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bubble")
        {
            AdjustToGrid(collision.collider.gameObject);

            foreach (var bubble in NearbyBubbles(transform.position))
            {
                AttachToBubble(bubble.gameObject);
            }

            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 1.0f;
            rigidBody.isKinematic = true;

            collider.radius /= RADIUS_FACTOR;
            gameObject.layer = LayerMask.NameToLayer(ATTACHED_LAYER);

            Destroy(this);
            EventDispatcher.Instance.Dispatch(new BubbleSettlingEvent());

            GetComponent<BubbleAttachments>().Model.CheckForMatches();
            EventDispatcher.Instance.Dispatch(new BubbleSettledEvent() { shooter = gameObject });
        }
    }

    private void AdjustToGrid(GameObject bubble)
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
        GetComponent<BubbleAttachments>().Attach(bubble);
    }

    private IEnumerable<Collider2D> NearbyBubbles(Vector2 location)
    {
        foreach (var hit in Physics2D.CircleCastAll(location, BUBBLE_SPACING, Vector2.up, 0.0f))
        {
            if ((hit.collider.gameObject != gameObject) && (hit.collider.gameObject.tag == "Bubble"))
            {
                yield return hit.collider;
            }
        }
    }

    private bool CanPlaceAtLocation(Vector2 location)
    {
        foreach (var hit in Physics2D.CircleCastAll(location, (BUBBLE_SPACING / 2.0f) * 0.9f, Vector2.up, 0.0f, LayerMask.GetMask("Game Objects", "Boundary")))
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

        foreach (var bubble in bubbles)
        {
            var bubblePosition = (Vector2)bubble.transform.position;

            for (var index = 0; index < 6; index++)
            {
                yield return new Vector2(
                    bubblePosition.x + Mathf.Cos(index * theta) * BUBBLE_SPACING,
                    bubblePosition.y + Mathf.Sin(index * theta) * BUBBLE_SPACING
                );
            }
        }
    }
}
