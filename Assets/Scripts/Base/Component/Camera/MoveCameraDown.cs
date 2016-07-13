using UnityEngine;
using System.Collections;

public class MoveCameraDown : MonoBehaviour
{
    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    [SerializeField]
    private float startDelay;

    private Collider2D castingBox;

    private void Start()
    {
        castingBox = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // var transform = gameView.transform;
        // var yTransform = (transform.position.y + ((Time.deltaTime * panSpeed) * -1));
        // transform.position = new Vector3(transform.position.x, yTransform);
        StartCoroutine(moveGameView());
    }

    private bool IsTouchingBubbles()
    {
        var bounds = castingBox.bounds;
        var origin = bounds.center;
        var size = bounds.max - bounds.min;
        var direction = Vector2.left;

        return Physics2D.BoxCastAll(origin, size, 0, direction, 0, 1 << (int)Layers.GameObjects).Length > 0;
    }

    private IEnumerator moveGameView()
    {
        yield return new WaitForSeconds(startDelay);
        while (IsTouchingBubbles())
        {
            var transform = gameView.transform;
            var yTransform = transform.position.y - (Time.deltaTime * panSpeed);
            transform.position = new Vector3(transform.position.x, yTransform);
            yield return null;
        }
    }
}