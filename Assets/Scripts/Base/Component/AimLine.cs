using UnityEngine;
using System.Collections.Generic;

public class AimLine : MonoBehaviour
{
    public Color Color
    {
        set
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.color = value;
            }
        }
    }

    public GameObject launchOrigin;
    public GameConfig config;
    public Texture2D texture;
    public float length;
    public float lineWidth;
    public float dotSpacing;
    public float moveSpeed;
    public float wallBounceDistance;

    private bool aiming = false;
    private Vector3 aimTarget;
    private List<Vector3> points;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    protected void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture;

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
    }

    protected void Update()
    {
        if (aiming)
        {
            RebuildMesh();
        }
    }

    protected void OnMouseDown()
    {
        meshRenderer.enabled = aiming = true;
        OnMouseDrag();
    }

    protected void OnMouseDrag()
    {
        if (aiming)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                aimTarget = hit.point;
            }

            GeneratePoints();
            RebuildMesh();
        }
    }

    protected void OnMouseUp()
    {
        meshRenderer.enabled = aiming = false;
    }

    private void GeneratePoints()
    {
        points = new List<Vector3>();

        Vector3 origin = launchOrigin.transform.position;
        int index = 0;
        var distance = length - config.bubbles.size;
        var direction = (Vector3)(aimTarget - origin).normalized;

        points.Add(origin + config.bubbles.size * direction);

        while (distance > 0.0f)
        {
            var hit = Physics2D.Raycast(points[index], direction, distance, LayerMask.GetMask(new string[] { "Game Objects", "Boundary" }));

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Bubble")
                {
                    points.Add(hit.point);
                    break;
                }
                else
                {
                    var span = hit.distance - config.bubbles.size * config.bubbles.shotColliderScale;
                    points.Add(points[index] + span * direction);
                    index++;
                    distance = wallBounceDistance;
                    direction = new Vector2(-direction.x, direction.y);
                }
            }
            else
            {
                points.Add(points[index] + distance * direction);
                break;
            }
        }

        points[0] = origin;
    }

    private void RebuildMesh()
    {
        var origin = launchOrigin.transform.position - launchOrigin.transform.localPosition;
        float offset = ((moveSpeed * Time.realtimeSinceStartup) % 1.0f) * dotSpacing;
        var halfSize = lineWidth / 2.0f;
        var count = points.Count - 1;

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();

        int vertexOffset = 0;

        for (var index = 0; index < count; index++)
        {
            var delta = (points[index + 1] - points[index]);
            var segmentLength = delta.magnitude;
            var direction = new Vector2(delta.x / segmentLength, delta.y / segmentLength);

            float x = points[index].x - origin.x + (direction.x * offset);
            float y = points[index].y - origin.y + (direction.y * offset);
            segmentLength -= offset;

            do
            {
                vertices.Add(new Vector3(x - halfSize, y + halfSize, 11.0f));
                vertices.Add(new Vector3(x + halfSize, y + halfSize, 11.0f));
                vertices.Add(new Vector3(x - halfSize, y - halfSize, 11.0f));
                vertices.Add(new Vector3(x + halfSize, y - halfSize, 11.0f));

                triangles.Add(vertexOffset + 0);
                triangles.Add(vertexOffset + 1);
                triangles.Add(vertexOffset + 2);

                triangles.Add(vertexOffset + 1);
                triangles.Add(vertexOffset + 3);
                triangles.Add(vertexOffset + 2);

                uvs.Add(new Vector2(0.0f, 0.0f));
                uvs.Add(new Vector2(1.0f, 0.0f));
                uvs.Add(new Vector2(0.0f, 1.0f));
                uvs.Add(new Vector2(1.0f, 1.0f));

                x += direction.x * dotSpacing;
                y += direction.y * dotSpacing;

                segmentLength -= dotSpacing;

                vertexOffset += 4;
            } while (segmentLength >= 0.0f);

            offset = -segmentLength;
        }

        var mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        meshFilter.mesh = mesh;
    }
}
