using UnityEngine;
using System.Collections.Generic;

sealed public class TrajectoryRotator : MonoBehaviour, UpdateReceiver
{
    private Transform myTransform;
    private Vector3 lastPosition;

    public void Awake()
    {
        myTransform = transform;
    }

    public void OnEnable()
    {
        if (myTransform != null)
        {
            GlobalState.UpdateService.Updates.Add(this);
            lastPosition = myTransform.position;
        }
    }

    public void OnDisable()
    {
        GlobalState.UpdateService.Updates.Remove(this);
    }

    public void OnDestroy()
    {
        OnDisable();
    }

    public void OnUpdate()
    {
        var position = myTransform.position;
        var delta = (position - lastPosition);

        if (delta.sqrMagnitude > Mathf.Epsilon)
        {
            var angle = Mathf.Atan2(delta.y, delta.x);

            myTransform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);
            lastPosition = position;
        }
    }
}
