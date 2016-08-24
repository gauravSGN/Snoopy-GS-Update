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

    public void Initialize()
    {
        GlobalState.EventService.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
    }

    public void Show(int multiplier, int score, Color color)
    {
        GlobalState.EventService.RemoveEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);

        transform.position = new Vector3(xAverage, yMin, transform.position.z);

        text.text = string.Format(string.Join("\n", lines), multiplier, score);
        GetComponent<Outline>().effectColor = color;
        GetComponent<Shadow>().effectColor = color;

        GetComponent<MoveToRegisteredCanvas>().MoveToCanvas();
        StartCoroutine(DestroyAfterSeconds(lingerTime));
    }

    private void OnBubbleDestroyed(BubbleDestroyedEvent gameEvent)
    {
        var position = gameEvent.bubble.transform.position;

        counter++;
        xAverage = ((xAverage * (counter - 1)) + position.x) / counter;
        yMin = (counter == 1) ? position.y : Mathf.Min(yMin, position.y);
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
