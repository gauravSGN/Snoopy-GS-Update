using Service;
using Registry;
using UnityEngine;
using System.Collections;

public class MoveToRegisteredCanvas : MonoBehaviour
{
    [SerializeField]
    private TransformUsage usage;

    [SerializeField]
    private bool moveOnAwake;

    [SerializeField]
    private bool useTargetAsParent;

    public void Start()
    {
        if (moveOnAwake)
        {
            MoveToCanvas();
        }
    }

    public void MoveToCanvas()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Transform target = null;

        while (target == null)
        {
            yield return null;
            target = GlobalState.Instance.Services.Get<TransformService>().Get(usage);
        }

        if (!useTargetAsParent)
        {
            target = target.GetComponentInParent<Canvas>().transform;
        }

        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        transform.SetParent(target, false);
        transform.position = screenPoint;
    }
}
