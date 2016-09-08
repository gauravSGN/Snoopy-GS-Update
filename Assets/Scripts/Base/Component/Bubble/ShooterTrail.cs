using UnityEngine;

sealed public class ShooterTrail : MonoBehaviour
{
    [SerializeField]
    private GameObject trailPrefab;

    private GameObject trail;

    public void Start()
    {
        trail = Instantiate(trailPrefab);
        trail.transform.localPosition = Vector3.zero;
        trail.transform.SetParent(transform, false);

        GlobalState.EventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        GlobalState.EventService.AddEventHandler<BubbleSettlingEvent>(OnBubbleSettling);
    }

    private void OnBubbleFired(BubbleFiredEvent gameEvent)
    {
        transform.SetParent(gameEvent.bubble.transform, false);
        transform.localPosition = Vector3.forward;
        trail.SetActive(true);
    }

    private void OnBubbleSettling()
    {
        transform.SetParent(null, true);
        trail.SetActive(false);
    }
}
