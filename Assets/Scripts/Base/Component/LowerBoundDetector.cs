using UnityEngine;
using System.Collections.Generic;

public class LowerBoundDetector : MonoBehaviour
{
    public GameObject gameView;
    public float panSpeed;

    public int bubbleCount = 0;
    private List<GameObject> bubbles = new List<GameObject>();

    protected void Update()
    {
        if ((bubbleCount == 0) && (gameView != null))
        {
            var transform = gameView.transform;

            if (transform.position.y < 0.0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * panSpeed);
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "Bubble") && !bubbles.Contains(collider.gameObject))
        {
            bubbles.Add(collider.gameObject);
            bubbleCount++;
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "Bubble") && bubbles.Contains(collider.gameObject))
        {
            bubbles.Remove(collider.gameObject);
            bubbleCount--;
        }
    }
}
