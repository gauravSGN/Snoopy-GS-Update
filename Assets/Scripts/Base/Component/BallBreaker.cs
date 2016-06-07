using UnityEngine;
using System.Collections;

public class BallBreaker : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        BubbleDeath death = collider.gameObject.GetComponent<BubbleDeath>();
        if(death)
        {
            Debug.Log("Starting death");
            //Debug.Break();
            StartCoroutine(death.TriggerDeathEffects());
        }
        else
        {
            Debug.Log("No death behaviour found");
            Destroy(collider.gameObject);
        }
    }
}
