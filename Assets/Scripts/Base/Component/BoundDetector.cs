using UnityEngine;
using System.Collections.Generic;

public class BoundDetector : MonoBehaviour
{
    private const float CEILING = 0.0f;

    public GameObject gameView;
    public float panSpeed;
    public int bubbleCount = 0;
    public Direction direction;

    public enum Direction
    {
        Up = -1,
        Down = 1,
    };

    public List<GameObject> bubbles = new List<GameObject>();

    protected void Update()
    {
        if ((gameView != null) &&
           (((direction == Direction.Down) && (bubbleCount == 0)) ||
            ((direction == Direction.Up) && (bubbleCount > 1))))
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
