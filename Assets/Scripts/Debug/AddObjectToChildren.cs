using UnityEngine;

public static class AddObjectToChildren
{
    public delegate bool Filter(GameObject gameObject);

    public static void AddObject(Transform transform, GameObject prefab, Filter filter = null)
    {
        for (int i = 0, c = transform.childCount; i < c; ++i)
        {
            var child = transform.GetChild(i);
            if (filter == null || filter(child.gameObject))
            {
                var instance = GameObject.Instantiate(prefab);
                var pos = transform.position;
                instance.transform.SetParent(child);
                instance.transform.localPosition = pos;
            }
        }
    }
}
