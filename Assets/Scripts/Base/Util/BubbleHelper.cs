using UnityEngine;
using System.Collections;

public class BubbleHelper
{
    private const float PI_OVER_3 = Mathf.PI / 3.0f;

    public static float FindClosestSnapAngle(GameObject moving, GameObject stationary)
    {
        var delta = moving.transform.position - stationary.transform.position;
        var angle = Mathf.Atan2(delta.y, delta.x);
        return Mathf.Round(angle / PI_OVER_3) * PI_OVER_3;
    }
}
