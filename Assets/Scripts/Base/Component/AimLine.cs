using UnityEngine;
using System.Collections.Generic;
using Service;
using System;

public class AimLine : InitializableBehaviour, UpdateReceiver
{
    [Serializable]
    public struct Settings
    {
        public int maxReflections;
        public float reflectionDistance;
    }

    private const int LAYER_MASK = (1 << (int)Layers.GameObjects | 1 << (int)Layers.Walls);
    private const int GAME_OBJECT_MASK = 1 << (int)Layers.GameObjects;

    public event Action<Vector2> Fire;

    public Color[] colors;

    [SerializeField]
    private GameObject launchOrigin;

    [SerializeField]
    private AimLineEventTrigger eventTrigger;

    private Vector3 aimTarget;
    private readonly List<Vector3> points = new List<Vector3>();
    private GameConfig.AimlineConfig aimlineConfig;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Settings? settings;

    public bool Aiming { get { return meshRenderer.enabled; } }
    public Vector3 Target { get { return aimTarget; } }

    override public void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = new Mesh();
        // Start disabled so Aiming is in correct state
        meshRenderer.enabled = false;

        base.Start();
    }

    public void OnUpdate()
    {
        RebuildMesh();
    }

    public void ModifyAimline(Settings newSettings)
    {
        settings = newSettings;
    }

    override public void Initialize()
    {
        aimlineConfig = GlobalState.Instance.Config.aimline;

        if (!settings.HasValue)
        {
            ResetReflections();
        }

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

        ResetReflections();
    }

    private void ResetReflections()
    {
        settings = aimlineConfig.normal;
    }

    private void GeneratePoints()
    {
        Vector3 origin = launchOrigin.transform.position;
        var config = GlobalState.Instance.Config;
        int index = 0;
        var distance = aimlineConfig.length - config.bubbles.size;
        var direction = (aimTarget - origin).normalized;
        var shooterRadius = config.bubbles.size * config.bubbles.shotColliderScale;
        int reflections = settings.Value.maxReflections;

        points.Clear();
        points.Add(origin + config.bubbles.size * direction * 2.0f);

        while (distance > 0.0f)
        {
            var hit = Physics2D.CircleCast(points[index], shooterRadius, direction, distance, LAYER_MASK);

            if (hit.collider != null)
            {
                if (Vector3.Dot(direction, ((Vector3)hit.point - points[index])) < 0.0f)
                {
                    // Make sure we're not trying to go backwards
                    break;
                }

                distance = hit.distance - shooterRadius * 0.5f;

                if (hit.collider.gameObject.tag == StringConstants.Tags.BUBBLES)
                {
                    // Push the aimline endpoint up against the edge of the bubble we collided with
                    distance += shooterRadius;
                }
            }

            points.Add(points[index] + distance * direction);

            if ((hit.collider != null) &&
                (reflections > 0) &&
                (hit.collider.gameObject.tag != StringConstants.Tags.BUBBLES))
            {
                // We've hit a wall
                --reflections;
                index++;

                // Make sure we shouldn't have hit a bubble as we bounce off the wall.  This should resolve the edge
                // case that allowed aiming around the end of a short row.
                if (Physics2D.OverlapCircle(points[index], config.bubbles.size / 2.0f, GAME_OBJECT_MASK) != null)
                {
                    points[index] = points[index] - direction * config.bubbles.size / 2.0f;
                    break;
                }

                distance = settings.Value.reflectionDistance;
                direction = new Vector2(-direction.x, direction.y);

                continue;
            }

            break;
        }

        points[0] = origin;
    }

    private void RebuildMesh()
    {
        var numberOfColors = colors.Length;
        var reverseStartingColorIndex = (int)((aimlineConfig.moveSpeed * Time.realtimeSinceStartup) % numberOfColors);
        var startingColorIndex = (numberOfColors - 1 - reverseStartingColorIndex);

        var uvs = new List<Vector2>();
        var triangles = new List<int>();
        var vertices = new List<Vector3>();
        var vertexColors = new List<Color>();

        var halfSize = aimlineConfig.lineWidth / 2.0f;
        int vertexOffset = 0;

        foreach (var point in GetPointsOnAimLine())
        {
            vertices.Add(new Vector3(point.x - halfSize, point.y + halfSize, 11.0f));
            vertices.Add(new Vector3(point.x + halfSize, point.y + halfSize, 11.0f));
            vertices.Add(new Vector3(point.x - halfSize, point.y - halfSize, 11.0f));
            vertices.Add(new Vector3(point.x + halfSize, point.y - halfSize, 11.0f));

            var colorIndex = ((vertexOffset / 4) + startingColorIndex) % numberOfColors;

            for (var i = 0; i < 4; i++)
            {
                vertexColors.Add(colors[colorIndex]);
            }

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
            colors = vertexColors.ToArray(),
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
