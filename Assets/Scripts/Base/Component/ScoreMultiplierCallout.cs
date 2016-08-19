using UnityEngine;
using System.Collections;

public class ScoreMultiplierCallout : MonoBehaviour
{
    [SerializeField]
    private TextMesh textMesh;

    [SerializeField]
    private string[] lines;

    private float xAverage;
    private float yMin;
    private int counter;

    public void Initialize()
    {
        GlobalState.EventService.AddEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);
    }

    public void Show(int multiplier, int score)
    {
        GlobalState.EventService.RemoveEventHandler<BubbleDestroyedEvent>(OnBubbleDestroyed);

        transform.position = new Vector3(xAverage, yMin, transform.position.z);
        textMesh.text = string.Format(string.Join("\n", lines), multiplier, score);

        StartCoroutine(DestroyAfterSeconds(1.0f));
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
