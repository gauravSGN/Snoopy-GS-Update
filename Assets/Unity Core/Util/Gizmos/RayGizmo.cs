using UnityEngine;
using System.Collections.Generic;

public class RayGizmo : FancyGizmo
{
    private struct Ray
    {
        public Ray(Vector3 start, Vector3 direction)
        {
            this.start = start;
            this.direction = direction;
        }
        public Vector3 start;
        public Vector3 direction;
    }

    public Vector3 direction;
    public bool drawArrow;

    private List<Ray> history = new List<Ray>();

    override protected void RecordHistory()
    {
        Ray toRecord = new Ray(transform.position, direction);
        history.Add(toRecord);
        if(history.Count > maxHistory)
        {
            history.RemoveAt(0);
        }
    }

    void OnDrawGizmos()
    {
        Ray toDraw = new Ray(transform.position, direction);
        DrawAt(toDraw, color);
        if(showHistory)
        {
            DrawHistory();
        }
    }

    private void DrawAt(Ray ray, Color currColor)
    {
        Vector3 start = ray.start + positionOffset;
        Vector3 end = start + ray.direction;
        Gizmos.color = currColor;
        Gizmos.DrawLine(start, end);
        if(drawArrow)
        {
            float arrowHeadAngle = 20.0f;
            float arrowHeadLength = 0.25f;
            Vector3 right = Quaternion.LookRotation(ray.direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(ray.direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * Vector3.forward;
            Gizmos.DrawRay(end, right * arrowHeadLength);
            Gizmos.DrawRay(end, left * arrowHeadLength);
        }
    }

    private void DrawHistory()
    {
        for(int i = 0; i < history.Count; ++i)
        {
            //at idx 0 use falloffColor at Count use color
            Color currColor = Color.Lerp(historyFalloffColor, color, (float)i/history.Count);
            DrawAt(history[i], currColor);
        }
    }
}
