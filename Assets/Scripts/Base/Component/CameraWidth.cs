using UnityEngine;
using System.Collections;

public class CameraWidth : MonoBehaviour
{
    public GameConfig config;

    protected void OnEnable()
    {
        var camera = Camera.main;
        camera.orthographicSize = (config.bubblesPerRow / camera.aspect * config.bubbleSize) / 2.0f;
    }
}
