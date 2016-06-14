using UnityEngine;
using UnityEditor;
using FSM;

[CustomEditor(typeof(State), true)]
public class StateEditor : Editor
{
    [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
    public static void DrawGizmo(State s, GizmoType type)
    {
        Vector3 off = new Vector3(1, 2, 1);
        Handles.Label(s.transform.position + off, s.name);
    }
}