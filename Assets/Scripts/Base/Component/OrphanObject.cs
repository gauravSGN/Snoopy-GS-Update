using UnityEngine;
using System.Collections;

public class OrphanObject : MonoBehaviour
{
    protected void OnEnable()
    {
        StartCoroutine(RemoveFromParent());
    }

    private IEnumerator RemoveFromParent()
    {
        yield return null;
        transform.SetParent(null, true);
    }
}
