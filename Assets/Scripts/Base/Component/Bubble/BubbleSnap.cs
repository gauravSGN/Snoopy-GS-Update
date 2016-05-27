using UnityEngine;
using System.Collections;

public class BubbleSnap : MonoBehaviour
{
    private const float BUBBLE_SPACING = 0.25f;
    private const float RADIUS_FACTOR = 0.5f;

    private Rigidbody2D rigidBody;
    private CircleCollider2D collider;

    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();

        collider.radius *= RADIUS_FACTOR;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bubble")
        {
            AdjustToGrid(collision.collider.gameObject);

            foreach (var hit in Physics2D.CircleCastAll((Vector2)transform.position, BUBBLE_SPACING, Vector2.up, 0.0f))
            {
                if ((hit.collider.gameObject != gameObject) && (hit.collider.gameObject.tag == "Bubble"))
                {
                    AttachToBubble(hit.collider.gameObject);
                }
            }

            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 1.0f;
            rigidBody.isKinematic = true;

            collider.radius /= RADIUS_FACTOR;

            Destroy(this);

            GetComponent<BubbleAttachments>().Model.CheckForMatches();
        }
    }

    private void AdjustToGrid(GameObject bubble)
    {
        var angle = BubbleHelper.FindClosestSnapAngle(gameObject, bubble);

        transform.position = bubble.transform.position + BUBBLE_SPACING * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void AttachToBubble(GameObject bubble)
    {
        GetComponent<BubbleAttachments>().Attach(bubble);
    }
}
