using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreMultiplierCallout : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private float lingerTime;

    [SerializeField]
    private string[] lines;

    private float xAverage;
    private float yMin;
    private int counter;
    private Text textComponent;

    public void Initialize()
    {
        GlobalState.EventService.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        textComponent = GetComponent<Text>();
    }

    public void Show(int multiplier, int score)
    {
        GlobalState.EventService.RemoveEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
        StartCoroutine(DelayedShow(multiplier, score));
    }

    private IEnumerator DelayedShow(int multiplier, int score)
    {
        yield return new WaitForSeconds(1f);

        transform.position = new Vector3(xAverage, yMin, transform.position.z);

        text.text = string.Format(string.Join("\n", lines), multiplier, score);

        GetComponent<MoveToRegisteredCanvas>().MoveToCanvas();

        while (transform.parent == null)
        {
            yield return null;
        }

        ClampToParent();

        var fadeTime = 0.2f;
        var timer = 0f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            Color color = textComponent.color;
            color.a = (timer / fadeTime);
            textComponent.color = color;
            yield return null;
        }

        timer = 0f;
        var fadeDelay = lingerTime - fadeTime;
        yield return new WaitForSeconds(fadeDelay);

        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            Color color = textComponent.color;
            color.a = 1 - (timer / fadeTime);
            textComponent.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnBubbleDestroyed(BubbleDestroyedEvent gameEvent)
    {
        var position = gameEvent.bubble.transform.position;

        counter++;
        xAverage = ((xAverage * (counter - 1)) + position.x) / counter;
        yMin = (counter == 1) ? position.y : Mathf.Min(yMin, position.y);
        // Hack for VS
        yMin += 1f;
    }

    private void ClampToParent()
    {
        var rectTransform = GetComponent<RectTransform>();
        var parentRect = (rectTransform.parent as RectTransform).rect;
        var myRect = rectTransform.rect;
        var position = rectTransform.localPosition;

        rectTransform.localPosition += new Vector3(
            ComputeRectOffset(myRect.xMin + position.x, myRect.xMax + position.x, parentRect.xMin, parentRect.xMax),
            ComputeRectOffset(myRect.yMin + position.y, myRect.yMax + position.y, parentRect.yMin, parentRect.yMax)
        );
    }

    private float ComputeRectOffset(float min, float max, float minBounds, float maxBounds)
    {
        var offset = 0.0f;

        if (min < minBounds)
        {
            offset = minBounds - min;
        }
        else if (max > maxBounds)
        {
            offset = maxBounds - max;
        }

        return offset;
    }
}
