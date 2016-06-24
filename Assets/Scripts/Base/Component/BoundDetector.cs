using UnityEngine;
using System.Collections.Generic;

public class BoundDetector : MonoBehaviour, UpdateReceiver
{
    private const float CEILING = 0.0f;

    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    [SerializeField]
    private Direction direction;

    [SerializeField]
    private readonly List<GameObject> bubbles = new List<GameObject>();

    private enum Direction
    {
        Up = -1,
        Down = 1,
    };

    public void OnUpdate()
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
        if ((collider.gameObject.tag == StringConstants.Tags.BUBBLES) && !bubbles.Contains(collider.gameObject))
        {
            bubbles.Add(collider.gameObject);

            if ((direction == Direction.Up) && (bubbles.Count == 1))
            {
                GlobalState.Instance.UpdateDispatcher.Updates.Add(this);
            }
            else if (direction == Direction.Down)
            {
                GlobalState.Instance.UpdateDispatcher.Updates.Remove(this);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == StringConstants.Tags.BUBBLES) && bubbles.Contains(collider.gameObject))
        {
            bubbles.Remove(collider.gameObject);

            if ((direction == Direction.Up) && (bubbles.Count == 0))
            {
                GlobalState.Instance.UpdateDispatcher.Updates.Remove(this);
            }
            else if ((direction == Direction.Down) && (bubbles.Count == 0))
            {
                GlobalState.Instance.UpdateDispatcher.Updates.Add(this);
            }
        }
    }
}
