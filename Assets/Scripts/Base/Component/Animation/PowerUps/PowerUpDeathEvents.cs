using UnityEngine;
using System.Collections;

public class PowerUpDeathEvents : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer additiveRenderer;

    [SerializeField]
    private SpriteRenderer background;

    [SerializeField]
    private Color colorStep;

    private bool keepTinting;

    public void StartBubbleTint()
    {
        background.color = Color.black;
        keepTinting = true;
        StartCoroutine(Tint());
    }

    public void EndBubbleTint()
    {
        keepTinting = false;
        additiveRenderer.sprite = background.sprite = null;
    }

    public IEnumerator Tint()
    {
        while (keepTinting)
        {
            background.color += colorStep;
            yield return null;
        }
    }
}
