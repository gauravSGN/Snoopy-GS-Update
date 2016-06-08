using UnityEngine;
using System.Collections.Generic;

public class LineGizmo : FancyGizmo {
    //cant use Tuple so we use this instead
    private struct EndPoints{
        public EndPoints(Vector3 start, Vector3 end){
            this.start = start;
            this.end = end;
        }
        public Vector3 start;
        public Vector3 end;
    }

    public Transform endPoint;
    public Vector3 endPointOffset;
    public bool drawArrow;

    private List<EndPoints> history = new List<EndPoints>();

    protected override void RecordHistory(){
        if(endPoint != null){
            EndPoints toRecord = new EndPoints(transform.position, endPoint.position);
            history.Add(toRecord);
            if(history.Count > maxHistory){
                history.RemoveAt(0);
            }
        }
    }

    void OnDrawGizmos(){
        if(endPoint != null){
            EndPoints toDraw = new EndPoints(transform.position, endPoint.position);
            DrawAt(toDraw, color);
            if(showHistory){
                DrawHistory();
            }
        }
    }

    private void DrawAt(EndPoints points, Color currColor){
        Vector3 start = points.start + positionOffset;
        Vector3 end = points.end + endPointOffset;
        Gizmos.color = currColor;
        Gizmos.DrawLine(start, end);
        if(drawArrow){
            float arrowHeadAngle = 20.0f;
            float arrowHeadLength = 0.25f;
            Vector3 direction = points.end - points.start;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * Vector3.forward;
            Gizmos.DrawRay(end, right * arrowHeadLength);
            Gizmos.DrawRay(end, left * arrowHeadLength);
        }
    }

    private void DrawHistory(){
        for(int i = 0; i < history.Count; ++i){
            //at idx 0 use falloffColor at Count use color
            Color currColor = Color.Lerp(historyFalloffColor, color, (float)i/history.Count);
            DrawAt(history[i], currColor);
        }
    }
}


