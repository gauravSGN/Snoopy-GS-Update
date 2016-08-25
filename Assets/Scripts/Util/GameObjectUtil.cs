using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectUtil
{
    public static void SetActive(IEnumerable<GameObject> objects, bool active)
    {
        foreach (var gameObject in objects)
        {
            gameObject.SetActive(active);
        }
    }
}
