using UnityEngine;
using System.Collections.Generic;

public class BoundDetector : MonoBehaviour
{
    private const float CEILING = 0.0f;

    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    [SerializeField]
    private Direction direction;

    [SerializeField]
    private List<GameObject> bubbles = new List<GameObject>();

    private enum Direction
    {
        Up = -1,
        Down = 1,
    };

    protected void Update()
    {
        var movingDown = (direction == Direction.Down) && (bubbles.Count == 0);
        var movingUp = (direction == Direction.Up) && (bubbles.Count > 0);

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
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "Bubble") && bubbles.Contains(collider.gameObject))
        {
            bubbles.Remove(collider.gameObject);
        }
    }
}
