using UnityEngine;

public class BubbleBreaker : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        BubbleDeath death = collider.gameObject.GetComponent<BubbleDeath>();
        collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

        if (death)
        {
            StartCoroutine(death.TriggerDeathEffects(BubbleDeathType.Cull));
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }
}
