using UnityEngine;

public class IdleDetector : MonoBehaviour
{
    [SerializeField]
    private float idleTimeout;

    private float lastActiveTime;

    public void Update()
    {
        var active = IsPlayerActive();
        var delta = Time.time - lastActiveTime;
        var idle = delta >= idleTimeout;
        var previouslyIdle = idle && ((delta - Time.deltaTime) >= idleTimeout);

        if (active)
        {
            lastActiveTime = Time.time;

            if (idle)
            {
                GlobalState.EventService.Dispatch(new PlayerIdleEvent(false));
            }
        }
        else if (idle && !previouslyIdle)
        {
            GlobalState.EventService.Dispatch(new PlayerIdleEvent(true));
        }
    }

    private bool IsPlayerActive()
    {
        return Input.anyKey || (Input.touchCount > 0);
    }
}
