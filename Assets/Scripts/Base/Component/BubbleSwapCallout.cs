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

        var targetAlpha = gameEvent.idle ? 1.0f : 0.0f;
        var fader = FadeAlpha(1.0f - targetAlpha, targetAlpha, 1.0f);

        if (gameEvent.idle)
        {
            StartCoroutine(fader);
            StartCoroutine(Spin(fader, c => true));
        }
        else
        {
            StartCoroutine(Spin(fader, c => c.MoveNext()));
        }
    }

    private IEnumerator Spin(IEnumerator coroutine, System.Func<IEnumerator, bool> predicate)
    {
        while (predicate(coroutine))
        {
            myTransform.localRotation *= Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, Vector3.forward);
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
}
