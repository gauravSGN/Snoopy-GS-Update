using Service;
using Registry;
using UnityEngine;
using System.Collections;

public class MoveToRegisteredCanvas : MonoBehaviour
{
    [SerializeField]
    private TransformUsage usage;

    public void Start()
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

        var canvas = target.GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            transform.SetParent(canvas.transform, false);
            transform.position = screenPoint;
        }
    }
}
