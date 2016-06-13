using UnityEngine;

public class BallBreaker : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        BubbleDeath death = collider.gameObject.GetComponent<BubbleDeath>();

        if (death)
        {
            StartCoroutine(death.TriggerDeathEffects());
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }
}
