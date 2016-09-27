using UnityEngine;
using System.Collections.Generic;

public class SwapActiveSprite : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites;

    private int index;

    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Swap()
    {
        index++;
        if (index == sprites.Count)
        {
            index = 0;
        }
        spriteRenderer.sprite = sprites[index];
    }
}
