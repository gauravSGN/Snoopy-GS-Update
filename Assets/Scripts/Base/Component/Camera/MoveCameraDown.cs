using Service;
using UnityEngine;

public class MoveCameraDown : MonoBehaviour, UpdateReceiver
{
    private const float CEILING = 0.0f;

    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    private Collider2D castingBox;

    protected void Start()
    {
        castingBox = gameObject.GetComponent<Collider2D>();
        GlobalState.Instance.EventDispatcher.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    public void OnUpdate()
    {
        var isMoving = !IsTouchingBubbles();

        if (isMoving)
        {
            moveGameView();
        }
        else
        {
            GlobalState.Instance.Services.Get<UpdateService>().Updates.Remove(this);
        }
    }

    private void moveGameView()
    {
        var transform = gameView.transform;

        if (transform.position.y < CEILING)
        {
            var yTransform = (transform.position.y + (Time.deltaTime * panSpeed));
            transform.position = new Vector3(transform.position.x, yTransform);
        }
    }

    private bool IsTouchingBubbles()
    {
        var bounds = castingBox.bounds;
        var origin = bounds.center;
        var size = bounds.max - bounds.min;
        var direction = Vector2.left;

        return Physics2D.BoxCastAll(origin, size, 0, direction, 0, 1 << (int)Layers.GameObjects).Length > 0;
    }

    private void OnReadyForNextBubbleEvent(ReadyForNextBubbleEvent gameEvent)
    {
        if (!IsTouchingBubbles())
        {
            GlobalState.Instance.Services.Get<UpdateService>().Updates.Add(this);
        }
    }
}
