using UnityEngine;

public class CopySprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] targetRenderers;

    [SerializeField]
    private string spriteName;

    protected void OnEnable()
    {
        if (transform.parent != null)
        {
            var spriteRenderers = transform.parent.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var renderer in spriteRenderers)
            {
                if (renderer.sprite != null && renderer.sprite.name.Contains(spriteName))
                {
                    foreach (var targetRenderer in targetRenderers)
                    {
                        targetRenderer.sprite = renderer.sprite;
                    }
                }
            }
        }
    }

    protected void Start()
    {
        OnEnable();
    }
}