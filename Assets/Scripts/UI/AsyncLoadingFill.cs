using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class AsyncLoadingFill : MonoBehaviour
{
    [SerializeField]
    private Slider fillBar;

    public void StartFill(AsyncOperation op, Action<GameObject> cb)
    {
        StartCoroutine(ProgressFill(op, cb));
    }

    private IEnumerator ProgressFill(AsyncOperation op, Action<GameObject> cb)
    {
        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.03f);
            fillBar.value = op.progress;
        }

        cb(gameObject);
    }
}
