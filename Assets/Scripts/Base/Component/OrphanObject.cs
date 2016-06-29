using UnityEngine;

public class OrphanObject : MonoBehaviour
{
    protected void Start()
    {
        gameObject.transform.parent = null;
    }
}