using UnityEngine;
using System.Collections.Generic;

public class BoundDetector : MonoBehaviour
{
    private const float CEILING = 0.0f;

    public GameObject gameView;
    public float panSpeed;
    public int bubbleCount;
    public Direction direction;

    public enum Direction
    {
        Up = -1,
        Down = 1,
    };

    public List<GameObject> bubbles = new List<GameObject>();

    protected void Update()
    {
        var movingUp = (direction == Direction.Down) && (bubbleCount == 0);
        var movingDown = (direction == Direction.Up) && (bubbleCount > 1);

        if ((gameView != null) && (movingUp || movingDown))
        {
            moveGameView();
        }
    }

    private void moveGameView()
    {
        var transform = gameView.transform;

        if ((transform.position.y < CEILING) || (direction == Direction.Up))
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
