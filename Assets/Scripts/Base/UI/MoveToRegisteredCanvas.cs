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

        var canvas = target.GetComponentInParent<Canvas>();

        if (!useTargetAsParent)
        {
            target = canvas.transform;
        }

        var myTransform = transform;
        var originalScale = myTransform.localScale;

        myTransform.SetParent(target, true);

        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            var screenPoint = (canvas.worldCamera ?? Camera.main).WorldToScreenPoint(myTransform.position);
            myTransform.position = screenPoint;
        }

        myTransform.localScale = originalScale;
    }
}
