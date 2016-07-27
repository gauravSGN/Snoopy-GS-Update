using UnityEngine;
using Spine;
using Spine.Unity;
using Service;
using System;

public class LauncherCharacterController : MonoBehaviour
{
    private const string ANGLE = "Angle";
    private const string AIMING = "Aiming";
    private const string FIRING = "Firing";

    public event Action OnAnimationFire;

    [SerializeField]
    private AimLineEventTrigger eventTrigger;

    [SerializeField]
    private GameObject launchOrigin;

    private Animator launcherAnimator;
    private Skeleton skeleton;

    public void AnimationFire()
    {
        if (OnAnimationFire != null)
        {
            OnAnimationFire();
        }
    }

    protected void Start()
    {
        launcherAnimator = gameObject.GetComponent<Animator>();
        skeleton = gameObject.GetComponent<SkeletonAnimator>().Skeleton;

        launcherAnimator.SetFloat(ANGLE, 90.0f);
        eventTrigger.MoveTarget += OnMoveTarget;

        var eventService = GlobalState.Instance.Services.Get<EventService>();
        eventService.AddEventHandler<BubbleFiringEvent>(OnBubbleFiring);
        eventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
    }

    private void OnBubbleFiring(BubbleFiringEvent bubbleFiringEvent)
    {
        launcherAnimator.SetBool(AIMING, false);
        launcherAnimator.SetTrigger(FIRING);
    }

    private void OnBubbleFired(BubbleFiredEvent bubbleFiredEvent)
    {
        launcherAnimator.SetFloat(ANGLE, 90f);
        skeleton.FlipX = false;
    }

    private void OnMoveTarget(Vector2 target)
    {
        launcherAnimator.SetBool(AIMING, true);

        var direction = (target - (Vector2)launchOrigin.transform.position).normalized;
        var angle = Vector2.Angle(Vector2.up, direction);

        skeleton.FlipX = direction.x > 0.01f;
        launcherAnimator.SetFloat(ANGLE, angle);
    }
}