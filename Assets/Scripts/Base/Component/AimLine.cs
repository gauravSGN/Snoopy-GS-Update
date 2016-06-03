using UnityEngine;
using System.Collections.Generic;

public class AimLine : MonoBehaviour
{
    public GameObject launchOrigin;
    public GameConfig config;
    public Texture2D texture;
    public float length;
    public float lineWidth;
    public float dotSpacing;
    public float moveSpeed;
    public float wallBounceDistance;

    private Vector3 aimTarget;
    private List<Vector3> points = new List<Vector3>();

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

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

    public bool Aiming
    {
        get
        {
            return meshRenderer.enabled;
        }
    }

    public Vector3 Target
    {
        get
        {
            return aimTarget;
        }
    }

    protected void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture;

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
    }

    protected void Update()
    {
        if (meshRenderer.enabled)
        {
            RebuildMesh();
        }
    }

    protected void OnMouseDown()
    {
        meshRenderer.enabled = true;
        OnMouseDrag();
    }

    protected void OnMouseDrag()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);

        meshRenderer.enabled = (hit.collider != null) && (hit.collider.gameObject == gameObject);

        if (meshRenderer.enabled)
        {
            aimTarget = hit.point;

            GeneratePoints();
        }
    }

    protected void OnMouseUp()
    {
        meshRenderer.enabled = false;
    }

    private void GeneratePoints()
    {
        points.Clear();

        Vector3 origin = launchOrigin.transform.position;
        int index = 0;
        var distance = length - config.bubbles.size;
        var direction = (aimTarget - origin).normalized;
        var shooterRadius = config.bubbles.size * config.bubbles.shotColliderScale / 2.0f;
        var layerMask = LayerMask.GetMask(new string[] { "Game Objects", "Boundary" });
        int reflections = 1;

        points.Add(origin + config.bubbles.size * direction * 2.0f);

        while (distance > 0.0f)
        {
            var hit = Physics2D.CircleCast(points[index], shooterRadius, direction, distance, layerMask);

            if (hit.collider != null)
            {
                // Make sure we're not trying to go backwards
                if (Vector3.Dot(direction, ((Vector3)hit.point - points[index])) < 0.0f)
                {
                    break;
                }

                distance = hit.distance - shooterRadius * 0.05f;
            }

            points.Add(points[index] + distance * direction);

            if ((hit.collider != null) && (hit.collider.gameObject.tag != "Bubble") && (reflections > 0))
            {
                --reflections;
                index++;
                distance = wallBounceDistance;
                direction = new Vector2(-direction.x, direction.y);

                continue;
            }

            break;
        }

        points[0] = origin;
    }

    private void RebuildMesh()
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();

        var halfSize = lineWidth / 2.0f;
        int vertexOffset = 0;

        foreach (var point in GetPointsOnAimLine())
        {
            vertices.Add(new Vector3(point.x - halfSize, point.y + halfSize, 11.0f));
            vertices.Add(new Vector3(point.x + halfSize, point.y + halfSize, 11.0f));
            vertices.Add(new Vector3(point.x - halfSize, point.y - halfSize, 11.0f));
            vertices.Add(new Vector3(point.x + halfSize, point.y - halfSize, 11.0f));

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

            vertexOffset += 4;
        }

        meshFilter.mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray(),
        };
    }

    private IEnumerable<Vector2> GetPointsOnAimLine()
    {
        var origin = launchOrigin.transform.position - launchOrigin.transform.localPosition;
        float offset = ((moveSpeed * Time.realtimeSinceStartup) % 1.0f) * dotSpacing;
        var count = points.Count - 1;

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
                yield return new Vector2(x, y);

                x += direction.x * dotSpacing;
                y += direction.y * dotSpacing;

                segmentLength -= dotSpacing;
            } while (segmentLength >= 0.0f);

            offset = -segmentLength;
        }
    }
}
