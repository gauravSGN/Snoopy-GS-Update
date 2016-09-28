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
            foreach (var renderer in transform.parent.GetComponentsInChildren<SpriteRenderer>(true))
            {
                UpdateSpriteRenderer(renderer);
            }
        }
    }

    protected void Start()
    {
        OnEnable();
    }

    private void UpdateSpriteRenderer(SpriteRenderer renderer)
    {
        if ((renderer.sprite != null) && renderer.sprite.name.Contains(spriteName))
        {
            foreach (var targetRenderer in targetRenderers)
            {
                targetRenderer.sprite = renderer.sprite;
            }
        }
    }
}