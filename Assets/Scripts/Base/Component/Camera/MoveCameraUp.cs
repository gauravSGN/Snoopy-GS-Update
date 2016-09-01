using UnityEngine;
using System.Collections;

public class MoveCameraUp : BaseMoveCamera
{
    [SerializeField]
    private Transform viewBoundary;

    override protected void Start()
    {
        base.Start();

        GlobalState.EventService.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    override protected IEnumerator MoveGameView()
    {
        GameObjectUtil.SetActive(disableOnMove, false);

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

        GameObjectUtil.SetActive(disableOnMove, true);
    }

    private void OnReadyForNextBubbleEvent()
    {
        if (!IsTouchingBubbles())
        {
            StartCoroutine(MoveGameView());
        }
    }
}
