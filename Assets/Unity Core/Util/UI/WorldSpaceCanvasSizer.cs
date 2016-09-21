using UnityEngine;

public class WorldSpaceCanvasSizer : MonoBehaviour
{
    [SerializeField]
    private int referenceWidth;

    [SerializeField]
    private int referenceHeight;

    protected void Start()
    {
        float referenceRatio = (float)referenceHeight / (float)referenceWidth;
        float aspectRatio = (float)Screen.height / (float)Screen.width;
        if (aspectRatio != referenceRatio)
        {
            float widthScalar = referenceRatio / aspectRatio;
            float newSize = referenceWidth * widthScalar;
            RectTransform rect = transform as RectTransform;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize);
        }
    }
}
