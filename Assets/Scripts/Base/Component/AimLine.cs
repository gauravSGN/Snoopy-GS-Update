using UnityEngine;
using System.Collections.Generic;

public class AimLine : MonoBehaviour
{
    public GameObject launchOrigin;
    public GameConfig config;
    public float length;

    private bool aiming = false;
    private Vector3 aimTarget;
    private LineRenderer lineRenderer;

    protected void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void Update()
    {
        if (aiming)
        {
            DrawAimLine(launchOrigin.transform.position, aimTarget);
        }
    }

    protected void OnMouseDown()
    {
        lineRenderer.enabled = aiming = true;
        OnMouseDrag();
    }

    protected void OnMouseDrag()
    {
        if (aiming)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit != null)
            {
                aimTarget = hit.point;
            }
        }
    }

    protected void OnMouseUp()
    {
        lineRenderer.enabled = aiming = false;
    }

    private void DrawAimLine(Vector3 origin, Vector3 target)
    {
        List<Vector3> points = new List<Vector3>();

        int index = 0;
        var distance = length - config.bubbles.size;
        var direction = (Vector3)(target - origin).normalized;

        points.Add(origin + config.bubbles.size * direction);

        while (distance > 0.0f)
        {
            var hit = Physics2D.Raycast(points[index], direction, distance, LayerMask.GetMask(new string[] { "Game Objects", "Boundary" }));

            if ((hit != null) && (hit.collider != null))
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
                    distance -= span;
                    direction = new Vector2(-direction.x, direction.y);
                }
            }
            else
            {
                points.Add(points[index] + distance * direction);
                break;
            }
        }

        lineRenderer.SetVertexCount(points.Count);
        lineRenderer.SetPositions(points.ToArray());
    }
}
