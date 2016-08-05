using UnityEngine;

public class SwapChildSprites : MonoBehaviour
{
    public void Swap()
    {
        var swappers = GetComponentsInChildren<SwapActiveSprite>();
        foreach (var s in swappers)
        {
            s.Swap();
        }
    }
}
