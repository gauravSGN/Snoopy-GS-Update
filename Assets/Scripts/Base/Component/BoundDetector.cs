using UnityEngine;
using System.Collections.Generic;

public class BoundDetector : MonoBehaviour
{
    private const float CEILING = 0.0f;

    public GameObject gameView;
    public float panSpeed;

    public enum Direction
    {
        up = -1,
        down = 1,
    };
    public Direction direction; 

    public int bubbleCount = 0;
    private List<GameObject> bubbles = new List<GameObject>();

    protected void Update()
    {
        if ((gameView != null) &&
           (((direction == Direction.down) && (bubbleCount == 0)) ||
            ((direction == Direction.up) && (bubbleCount > 1))))
        {
            moveGameView();
        }
    }

    private void moveGameView()
    {
        var transform = gameView.transform;

        if ((transform.position.y < CEILING))
        {
            var yTransform = (transform.position.y + ((Time.deltaTime * panSpeed) * (int)direction));
            transform.position = new Vector3(transform.position.x, yTransform);
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
