﻿using UnityEngine;
using System.Collections;

public class BubbleSnap : MonoBehaviour
{
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bubble")
        {
            AdjustToGrid(collision.collider.gameObject);

            foreach (var hit in Physics2D.CircleCastAll((Vector2)transform.position, 0.3f, Vector2.up, 0.0f))
            {
                if ((hit.collider.gameObject != gameObject) && (hit.collider.gameObject.tag == "Bubble"))
                {
                    AttachToBubble(hit.collider.gameObject);
                }
            }

            var rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 1.0f;

            Destroy(this);

            GetComponent<BubbleAttachments>().Model.CheckForMatches();
        }
    }

    private void AdjustToGrid(GameObject bubble)
    {
        var angle = BubbleHelper.FindClosestSnapAngle(gameObject, bubble);

        transform.position = bubble.transform.position + 0.3f * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void AttachToBubble(GameObject bubble)
    {
        GetComponent<BubbleAttachments>().Attach(bubble);
    }
}
