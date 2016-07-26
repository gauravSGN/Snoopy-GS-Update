using UnityEngine;
using Spine.Unity;
using Service;
using System;

public class LauncherCharacterController : MonoBehaviour
{
    public event Action OnAnimationFire;

    [SerializeField]
    private GameObject launcherCharacter;

    [SerializeField]
    private AimLineEventTrigger eventTrigger;

    [SerializeField]
    private GameObject launchOrigin;

    [SerializeField]
    private string bubbleAttachment;

    private Animator launcherAnimator;
    private Vector2 startingVector;
    private Spine.Skeleton skeleton;

    public void AnimationFire()
    {
        if (OnAnimationFire != null)
        {
            OnAnimationFire();
        }
    }

    protected void Start()
    {
        launcherAnimator = launcherCharacter.GetComponent<Animator>();
        skeleton = launcherCharacter.GetComponent<SkeletonAnimator>().Skeleton;
        skeleton.SetAttachment(bubbleAttachment, null);

        launcherAnimator.SetFloat("Angle", 90.0f);
        eventTrigger.MoveTarget += OnMoveTarget;

        var eventService = GlobalState.Instance.Services.Get<EventService>();
        eventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
    }


    private void OnInputToggle(InputToggleEvent inputToggleEvent)
    {
        if (launcherAnimator != null && skeleton != null)
        {
            if (!inputToggleEvent.enabled)
            {
                launcherAnimator.SetTrigger("Firing");
                launcherAnimator.SetFloat("Angle", 90f);

            }
            else
            {
                skeleton.FlipX = false;
            }
        }
    }

    private void OnMoveTarget(Vector2 target)
    {
        var direction = (target - (Vector2)launchOrigin.transform.position).normalized;
        var angle = Vector2.Angle(Vector2.up, direction);
        skeleton.FlipX = direction.x > 0.01f;

        Debug.Log(angle);
        launcherAnimator.SetFloat("Angle", angle);
    }
}