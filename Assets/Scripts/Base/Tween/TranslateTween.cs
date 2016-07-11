using UnityEngine;

public class TranslateTween : MonoBehaviour
{
    [SerializeField]
    private float xOffset;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    private float duration;

    public void PlayTweenForward()
    {
        var mtx = transform.localToWorldMatrix;
        var vec = mtx.MultiplyVector(new Vector3(xOffset, yOffset, 0));
        transform.positionTo(duration, vec, true);
    }

    public void PlayTweenBack()
    {
        var mtx = transform.localToWorldMatrix;
        var vec = mtx.MultiplyVector(new Vector3(-xOffset, -yOffset, 0));
        transform.positionTo(duration, vec, true);
    }
}
