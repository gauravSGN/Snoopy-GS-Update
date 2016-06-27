using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AsyncLoadingFill : MonoBehaviour
{
    [SerializeField]
    private Slider fillBar;

    public void StartFill(AsyncOperation op)
    {
        StartCoroutine(ProgressFill(op));
    }

    private IEnumerator ProgressFill(AsyncOperation op)
    {
        while (!op.isDone)
        {
            fillBar.value = op.progress;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    }
}
