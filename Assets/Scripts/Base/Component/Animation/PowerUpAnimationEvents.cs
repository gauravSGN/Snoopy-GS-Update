using UnityEngine;
using System.Collections.Generic;

public class PowerUpAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> startKiteSparkle01;

    public void StartKiteSparkle01()
    {
        foreach (var particleSystem in startKiteSparkle01)
        {
            particleSystem.SetActive(true);
        }
    }
}