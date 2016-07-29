using UnityEngine;
using System.Collections;

public class MoveCameraDown : BaseMoveCamera
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(MoveGameView());
    }

    override protected IEnumerator MoveGameView()
    {
        GameObjectUtil.DisableObjects(disableOnMove);

        yield return new WaitForSeconds(startDelay);

        var transform = gameView.transform;
        while (IsTouchingBubbles())
        {
            var yTransform = transform.position.y - (Time.deltaTime * panSpeed);
            transform.position = new Vector3(transform.position.x, yTransform);
            yield return null;
        }

        GameObjectUtil.EnableObjects(disableOnMove);
    }
}
