using UnityEngine;
using System.Collections.Generic;

public class BoxGizmo : FancyGizmo
{
    public Vector3 size = new Vector3(1f,1f,1f);
    public bool wireframe = false;

    private List<Vector3> history = new List<Vector3>();

    override protected void RecordHistory()
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
            Gizmos.DrawWireCube(position, size);
        }
        else
        {
            Gizmos.DrawCube(position, size);
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
