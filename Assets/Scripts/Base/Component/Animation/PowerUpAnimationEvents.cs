using UnityEngine;
using System.Collections.Generic;

public class PowerUpAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> startKiteSparkle01;

    // Event names need to match exactly what is in the spine animation data or they
    // won't work without some extra manual intervention. I talked to Eric about more
    // generic naming of events, so hopefully we can change this name before too long
    public void StartKiteSparkle01()
    {
        foreach (var particleSystem in startKiteSparkle01)
        {
            particleSystem.SetActive(true);
        }
    }
}