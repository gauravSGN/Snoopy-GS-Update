using UnityEngine;

public class SortOrderSetter : MonoBehaviour
{
    [SerializeField]
    private string SortingLayerName = "Default";

    [SerializeField]
    private int SortingOrder = 0;

    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer)
        {
            renderer.sortingLayerName = SortingLayerName;
            renderer.sortingOrder = SortingOrder;
        }
    }
}
