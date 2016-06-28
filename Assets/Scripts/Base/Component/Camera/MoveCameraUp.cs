using UnityEngine;

public class MoveCameraUp : MonoBehaviour
{
    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private float panSpeed;

    public void OnTriggerStay2D(Collider2D collider)
    {
        var transform = gameView.transform;
        var yTransform = (transform.position.y + ((Time.deltaTime * panSpeed) * -1));
        transform.position = new Vector3(transform.position.x, yTransform);
    }
}