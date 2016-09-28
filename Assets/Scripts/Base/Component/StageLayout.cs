using UnityEngine;

public class StageLayout : InitializableBehaviour
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

    override public void Initialize()
    {
        SetWallLocation(leftWall, -1.0f);
        SetWallLocation(rightWall, 1.0f);
    }

    private void SetWallLocation(BoxCollider2D wall, float direction)
    {
        float ySize = gameCamera.orthographicSize * wallScale;
        var config = GlobalState.Instance.Config;
        float x = direction * ((config.bubbles.numPerRow / 2 * config.bubbles.size) + wall.size.x / 2.0f);
        float y = (ySize / 2) - (gameCamera.orthographicSize);

        wall.transform.position = new Vector3(x, y, -2.0f);
        wall.size = new Vector2(wall.size.x, ySize);
    }
}
