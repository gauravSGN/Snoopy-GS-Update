﻿using UnityEngine;

public class StageLayout : MonoBehaviour
{
    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private BoxCollider2D leftWall;

    [SerializeField]
    private BoxCollider2D rightWall;

    [SerializeField]
    private float wallScale;

    protected void OnEnable()
    {
        SetCameraSize();
        ScaleBackgroundToCamera();
        SetWallLocation(leftWall, -1.0f);
        SetWallLocation(rightWall, 1.0f);
    }

    private void SetCameraSize()
    {
        gameCamera.aspect = 10.0f / 16.0f;
        var viewportWidth = gameCamera.aspect * Screen.height / Screen.width;
        var config = GlobalState.Instance.Config;
        gameCamera.rect = new Rect(
            (1.0f - viewportWidth) / 2.0f,
            0.0f,
            viewportWidth,
            1.0f
        );
        gameCamera.orthographicSize = (config.bubbles.numPerRow / gameCamera.aspect * config.bubbles.size) / 2.0f;
    }

    private void ScaleBackgroundToCamera()
    {
        var sprite = background.GetComponent<SpriteRenderer>().sprite;
        var cameraWidth = gameCamera.aspect * gameCamera.orthographicSize * 2.0f;

        background.transform.localScale = new Vector3(
            100.0f * cameraWidth / sprite.rect.width,
            100.0f * 2.0f * gameCamera.orthographicSize / sprite.rect.height
        );
    }

    private void SetWallLocation(BoxCollider2D wall, float direction)
    {
        float ySize = gameCamera.orthographicSize * wallScale;
        var config = GlobalState.Instance.Config;
        float x = direction * ((config.bubbles.numPerRow / 2 * config.bubbles.size) + wall.size.x / 2.0f);
        float y = (ySize / 2) - (gameCamera.orthographicSize);

        wall.transform.position = new Vector3(x, y);
        wall.size = new Vector2(wall.size.x, ySize);
    }
}
