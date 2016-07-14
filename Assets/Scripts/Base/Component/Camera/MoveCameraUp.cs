using Service;
using UnityEngine;
using System.Collections;

public class MoveCameraUp : MonoBehaviour
{
    private const float CEILING = 0.22f;

    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    [SerializeField]
    private float startDelay;

    private Collider2D castingBox;

    protected void Start()
    {
        castingBox = gameObject.GetComponent<Collider2D>();
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    private IEnumerator moveGameView()
    {
        yield return new WaitForSeconds(startDelay);
        var transform = gameView.transform;
        while (!IsTouchingBubbles() && transform.position.y < CEILING)
        {
            var yTransform = transform.position.y + (Time.deltaTime * panSpeed);
            transform.position = new Vector3(transform.position.x, yTransform);
            yield return null;
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
            StartCoroutine(moveGameView());
        }
    }
}
