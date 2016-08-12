using FTUE;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Lightbox))]
public class LightboxInspector : Editor
{
    public void OnSceneGUI()
    {
        var lightbox = target as Lightbox;
        var transform = lightbox.GetComponent<RectTransform>();

        for (int polyIndex = 0, polyCount = lightbox.Polygons.Count; polyIndex < polyCount; polyIndex++)
        {
            var polygon = lightbox.Polygons[polyIndex];
            for (int vertIndex = 0, vertCount = polygon.vertices.Count; vertIndex < vertCount; vertIndex++)
            {
                var point = transform.TransformPoint(polygon.vertices[vertIndex]);
                var result = Handles.FreeMoveHandle(point, Quaternion.identity, 3.0f, Vector3.one, Handles.DotCap);

                if (result != point)
                {
                    Undo.RecordObject(lightbox, "Move");
                    polygon.vertices[vertIndex] = transform.InverseTransformPoint(result);
                    lightbox.OnValidate();
                }
            }
        }
    }
}
