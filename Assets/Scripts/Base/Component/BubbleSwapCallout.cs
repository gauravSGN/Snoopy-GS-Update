using UnityEngine;
using System.Collections;

sealed public class BubbleSwapCallout : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private SpriteRenderer[] sprites;

    private Transform myTransform;

    public void Start()
    {
        myTransform = transform;

        GlobalState.EventService.AddEventHandler<PlayerIdleEvent>(OnPlayerIdle);
    }

    private void OnPlayerIdle(PlayerIdleEvent gameEvent)
    {
        StopAllCoroutines();

        StartCoroutine(gameEvent.idle ? PlayIdle() : PlayActive());
    }

    private IEnumerator PlayIdle()
    {
        StartCoroutine(FadeAlpha(0.0f, 1.0f, 1.0f));

        while (true)
        {
            ApplyRotation();
            yield return null;
        }
    }

    private IEnumerator PlayActive()
    {
        var coroutine = FadeAlpha(1.0f, 0.0f, 1.0f);

        while (coroutine.MoveNext())
        {
            ApplyRotation();
            yield return null;
        }
    }

    private IEnumerator FadeAlpha(float start, float end, float duration)
    {
        var time = 0.0f;

        do
        {
            yield return null;

            time += Time.deltaTime;
            var alpha = Mathf.Lerp(start, end, time / duration);

            foreach (var sprite in sprites)
            {
                sprite.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            }
        } while (time <= duration);
    }

    private void ApplyRotation()
    {
        myTransform.localRotation *= Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, Vector3.forward);
    }
}
