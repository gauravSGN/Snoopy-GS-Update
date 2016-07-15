using UnityEngine;
using System.Collections.Generic;

public static class GameObjectUtil
{
    public static void DisableObjects(List<GameObject> toDisable)
    {
        for(int i = 0, count = toDisable.Count; i < count; ++i)
        {
            toDisable[i].SetActive(false);
        }
    }

    public static void EnableObjects(List<GameObject> toEnable)
    {
        for(int i = 0, count = toEnable.Count; i < count; ++i)
        {
            toEnable[i].SetActive(true);
        }
    }
}
