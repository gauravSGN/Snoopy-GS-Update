using UnityEngine;
using System.Collections.Generic;

public class SphereGizmo : FancyGizmo
{
    public float radius = 1f;
    public bool wireframe = false;

    private List<Vector3> history = new List<Vector3>();

    protected override void RecordHistory()
    {
        history.Add(transform.position);
        if(history.Count > maxHistory)
        {
            history.RemoveAt(0);
        }
    }

    void OnDrawGizmos()
    {
        DrawAt(transform.position, color);
        if(showHistory)
        {
            DrawHistory();
        }
    }

    private void DrawAt(Vector3 pos, Color currColor)
    {
        Vector3 position = pos + positionOffset;
        Gizmos.color = currColor;
        if(wireframe)
        {
            Gizmos.DrawWireSphere(position, radius);
        }
        else
        {
            Gizmos.DrawSphere(position, radius);
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
