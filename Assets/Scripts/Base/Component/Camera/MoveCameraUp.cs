using Service;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCameraUp : MonoBehaviour
{
    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    [SerializeField]
    private float startDelay;

    [SerializeField]
    private Transform viewBoundary;

    [SerializeField]
    List<GameObject> disableOnMove;

    private Collider2D castingBox;

    protected void Start()
    {
        castingBox = gameObject.GetComponent<Collider2D>();
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    private IEnumerator MoveGameView()
    {
        GameObjectUtil.DisableObjects(disableOnMove);

        yield return new WaitForSeconds(startDelay);

        var transform = gameView.transform;
        var maxY = viewBoundary.position.y;
        while (!IsTouchingBubbles() && transform.position.y < maxY)
        {
            var yTransform = transform.position.y + (Time.deltaTime * panSpeed);
            transform.position = new Vector3(transform.position.x, yTransform, transform.position.z);
            yield return null;
        }
        if(transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }

        GameObjectUtil.EnableObjects(disableOnMove);
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
            StartCoroutine(MoveGameView());
        }
    }
}
