using UnityEngine;
using System.Collections.Generic;
using Service;
using System;

public class AimLine : MonoBehaviour, UpdateReceiver
{
    public event Action<Vector2> Fire;

    [SerializeField]
    private GameObject launchOrigin;

    [SerializeField]
    private AimLineEventTrigger eventTrigger;

    private Vector3 aimTarget;
    private readonly List<Vector3> points = new List<Vector3>();
    private GameConfig.AimlineConfig aimlineConfig;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    public bool Aiming { get { return meshRenderer.enabled; } }
    public Vector3 Target { get { return aimTarget; } }

    public Color Color
    {
        get { return meshRenderer.material.color; }
        set { meshRenderer.material.color = value; }
    }

    public void OnUpdate()
    {
        RebuildMesh();
    }

    protected void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = new Mesh();
        aimlineConfig = GlobalState.Instance.Config.aimline;

        eventTrigger.StartAiming += OnStartAiming;
        eventTrigger.StopAiming += OnStopAiming;
        eventTrigger.MoveTarget += OnMoveTarget;
        eventTrigger.Fire += OnFire;
    }

    private void OnStartAiming()
    {
        meshRenderer.enabled = true;
        GlobalState.Instance.Services.Get<UpdateService>().Updates.Add(this);
    }

    private void OnStopAiming()
    {
        meshRenderer.enabled = false;
        GlobalState.Instance.Services.Get<UpdateService>().Updates.Remove(this);
    }

    private void OnMoveTarget(Vector2 target)
    {
        aimTarget = target;
        GeneratePoints();
    }

    private void OnFire()
    {
        if (Fire != null)
        {
            Fire(aimTarget);
        }
    }

    private void GeneratePoints()
    {
        points.Clear();

        Vector3 origin = launchOrigin.transform.position;
        var config = GlobalState.Instance.Config;
        int index = 0;
        var distance = aimlineConfig.length - config.bubbles.size;
        var direction = (aimTarget - origin).normalized;
        // Make the cast size slightly larger than the actual shooter size to avoid false negatives
        var shooterRadius = (config.bubbles.size * config.bubbles.shotColliderScale / 1.4f);
        var layerMask = (1 << (int)Layers.GameObjects | 1 << (int)Layers.Walls);
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

                distance = hit.distance - shooterRadius * 0.5f;

                points.Add(points[index] + distance * direction);

                if ((hit.collider.gameObject.tag != StringConstants.Tags.BUBBLES) && (reflections > 0))
                {
                    --reflections;
                    index++;
                    distance = aimlineConfig.wallBounceDistance;
                    direction = new Vector2(-direction.x, direction.y);

                    continue;
                }
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

        var halfSize = aimlineConfig.lineWidth / 2.0f;
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
        float offset = ((aimlineConfig.moveSpeed * Time.realtimeSinceStartup) % 1.0f) * aimlineConfig.dotSpacing;
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

                x += direction.x * aimlineConfig.dotSpacing;
                y += direction.y * aimlineConfig.dotSpacing;

                segmentLength -= aimlineConfig.dotSpacing;
            } while (segmentLength >= 0.0f);

            offset = -segmentLength;
        }
    }
}
