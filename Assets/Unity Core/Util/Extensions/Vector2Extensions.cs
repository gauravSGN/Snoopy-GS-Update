using UnityEngine;
using System.Collections.Generic;

namespace Geometry
{
    static public class Vector2Extensions
    {
        static public float Cross(this Vector2 vector, Vector2 other)
        {
            return vector.x * other.y - vector.y * other.x;
        }
    }
}
