using Service;
using UnityEngine;
using System.Collections;

public class MoveCameraUp : BaseMoveCamera
{
    [SerializeField]
    private Transform viewBoundary;

    override protected void Start()
    {
        base.Start();

        var eventService = GlobalState.Instance.Services.Get<EventService>();
        eventService.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    override protected IEnumerator MoveGameView()
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

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }

        GameObjectUtil.EnableObjects(disableOnMove);
    }

    private void OnReadyForNextBubbleEvent(ReadyForNextBubbleEvent gameEvent)
    {
        if (!IsTouchingBubbles())
        {
            StartCoroutine(MoveGameView());
        }
    }
}
