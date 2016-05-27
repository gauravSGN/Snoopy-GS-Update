using UnityEngine;
using System.Collections;

public class StageLayout : MonoBehaviour
{
    public GameConfig config;
    public Camera gameCamera;
    public GameObject background;
    public BoxCollider2D leftWall;
    public BoxCollider2D rightWall;

    protected void Start()
    {
        SetCameraSize();
        ScaleBackgroundToCamera();
        SetWallLocation(leftWall, -1.0f);
        SetWallLocation(rightWall, 1.0f);
    }


    private void SetCameraSize()
    {
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
        float x = direction * ((config.bubbles.numPerRow / 2 * config.bubbles.size) + wall.size.x / 2.0f);
        wall.transform.position = new Vector3(x, 0.0f);
        wall.size = new Vector2(wall.size.x, gameCamera.orthographicSize * 2.0f);
    }
}
