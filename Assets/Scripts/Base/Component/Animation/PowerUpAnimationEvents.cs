using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Registry;
using Service;

public class PowerUpAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> startKiteSparkle01;

    [SerializeField]
    private TransformUsage usage = TransformUsage.YellowPowerUpReturnTarget;

    [SerializeField]
    private GameObject returningItem;

    [SerializeField]
    private float returnTime;

    // Event names need to match exactly what is in the spine animation data or they
    // won't work without some extra manual intervention. I talked to Eric about more
    // generic naming of events, so hopefully we can change this name before too long
    public void StartKiteSparkle01()
    {
        foreach (var particleSystem in startKiteSparkle01)
        {
            particleSystem.SetActive(true);
        }
    }

    public void ItemReturn()
    {
        var eventService = GlobalState.EventService;
        eventService.AddEventHandler<PowerUpReturningEvent>(OnReturning);
        eventService.AddEventHandler<PowerUpReturnedEvent>(OnReturned);
        returningItem.GetComponentInChildren<SpriteRenderer>().enabled = true;
        eventService.Dispatch<PowerUpReturnEvent>(new PowerUpReturnEvent());
    }

    private void OnReturning()
    {
        StartCoroutine(ReturnToTarget(GlobalState.Instance.Services.Get<TransformService>().Get(usage)));
        GlobalState.EventService.RemoveEventHandler<PowerUpReturningEvent>(OnReturning);
        Debug.Log("On Returning called.");
    }

    private void OnReturned()
    {
        GlobalState.EventService.RemoveEventHandler<PowerUpReturnedEvent>(OnReturned);
        returningItem.transform.SetParent(gameObject.transform);
        returningItem.transform.localPosition = Vector3.forward;
        returningItem.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    private IEnumerator ReturnToTarget(Transform target)
    {
        var runTime = 0f;
        var speed = Vector3.Distance(returningItem.transform.position, target.position) / returnTime;

        while (runTime <= returnTime)
        {
            yield return null;

            var elapsedTime = Time.deltaTime;
            var distance = speed * elapsedTime;
            returningItem.transform.position = Vector3.MoveTowards(returningItem.transform.position,
                                                                   target.position, distance);

            if (returningItem.transform.position == target.position)
            {
                returningItem.transform.SetParent(target.transform);
                break;
            }

            runTime += elapsedTime;
        }
    }
}
