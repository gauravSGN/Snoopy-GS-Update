using UnityEngine;
using System.Collections.Generic;

sealed public class TrajectoryRotator : MonoBehaviour, UpdateReceiver
{
    [SerializeField]
    private float offset;

    [SerializeField]
    private bool invert;

    private Transform myTransform;
    private Vector3 lastPosition;
    private ParticleSystem system;

    public void Awake()
    {
        myTransform = transform;
        system = GetComponent<ParticleSystem>();
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
            var angle = (Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x) + offset) * (invert ? -1.0f : 1.0f);

            myTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            system.startRotation = angle * Mathf.Deg2Rad;
            lastPosition = position;
        }
    }
}
