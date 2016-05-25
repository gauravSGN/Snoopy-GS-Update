using UnityEngine;
using System.Collections;

public class ScaleToCameraView : MonoBehaviour
{
    void Start()
    {
        var camera = Camera.main;
        var sprite = GetComponent<SpriteRenderer>().sprite;
        var cameraWidth = camera.aspect * camera.orthographicSize * 2.0f;
        transform.localScale = new Vector3(100.0f * cameraWidth / sprite.rect.width, 100.0f * 2.0f * camera.orthographicSize / sprite.rect.height);
    }
}
