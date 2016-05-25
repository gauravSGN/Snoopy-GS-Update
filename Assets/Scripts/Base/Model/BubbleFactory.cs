using UnityEngine;
using System;
using System.Collections.Generic;

public class BubbleFactory : ScriptableObject
{
    [Serializable]
    public class BubbleInfo
    {
        public BubbleType type;
        public GameObject prefab;
    }

    public List<BubbleInfo> bubbles;

    private Dictionary<BubbleType, BubbleInfo> lookup;

    public GameObject CreateBubbleByType(BubbleType type)
    {
        var info = GetBubbleInfoByType(type);
        return Instantiate(info.prefab);
    }

    private BubbleInfo GetBubbleInfoByType(BubbleType type)
    {
        if (lookup == null)
        {
            CreateLookupTable();
        }

        return lookup[type];
    }

    private void CreateLookupTable()
    {
        lookup = new Dictionary<BubbleType, BubbleInfo>();

        foreach (var info in bubbles)
        {
            lookup.Add(info.type, info);
        }
    }
}
