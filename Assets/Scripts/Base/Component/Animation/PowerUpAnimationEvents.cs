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
        GlobalState.EventService.Dispatch<PowerUpItemReturnEvent>(new PowerUpItemReturnEvent());
        StartCoroutine(ReturnToTarget(GlobalState.Instance.Services.Get<TransformService>().Get(usage)));
    }

    private IEnumerator ReturnToTarget(Transform target)
    {
        var runTime = 0f;
        var speed = Vector3.Distance(transform.position, target.position) / returnTime;
        transform.scaleTo(returnTime, 0.7f);

        while (runTime <= returnTime)
        {
            yield return null;

            var elapsedTime = Time.deltaTime;
            var distance = speed * elapsedTime;

            transform.position = Vector3.MoveTowards(transform.position, target.position, distance);
            runTime += elapsedTime;
        }


        transform.parent = target;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
