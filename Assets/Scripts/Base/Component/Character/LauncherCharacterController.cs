using UnityEngine;
using Spine;
using Spine.Unity;
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
        eventTrigger.StartAiming += OnStartAiming;
        eventTrigger.StopAiming += OnStopAiming;
        eventTrigger.MoveTarget += OnMoveTarget;


        GlobalState.EventService.AddEventHandler<BubbleFiringEvent>(OnBubbleFiring);
        GlobalState.EventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        GlobalState.EventService.AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
        GlobalState.EventService.AddEventHandler<ShotsRemainingEvent>(OnShotsRemaining);

    }

    private void OnBubbleFiring(BubbleFiringEvent bubbleFiringEvent)
    {
        launcherAnimator.SetBool(AIMING, false);
        launcherAnimator.SetTrigger(FIRING);
    }

    private void OnBubbleFired(BubbleFiredEvent bubbleFiredEvent)
    {
        launcherAnimator.SetFloat(ANGLE, 90f);
    }

    private void OnStartAiming()
    {
        launcherAnimator.SetBool(AIMING, true);
    }

    private void OnStopAiming()
    {
        launcherAnimator.SetBool(AIMING, false);
    }

    private void OnMoveTarget(Vector2 target)
    {
        var direction = (target - (Vector2)launchOrigin.transform.position).normalized;
        var angle = Vector2.Angle(Vector2.up, direction);

        skeleton.FlipX = direction.x > 0.01f;
        launcherAnimator.SetFloat(ANGLE, angle);
    }

    private void OnShotsRemaining(ShotsRemainingEvent gameEvent)
    {
        if (gameEvent.shots == 10)
        {
            launcherAnimator.SetBool("LosingLevel", true);
        }
    }

    private void OnLevelComplete(LevelCompleteEvent gameEvent)
    {
        launcherAnimator.SetBool("WonLevel", gameEvent.Won);
        launcherAnimator.SetBool("LostLevel", !gameEvent.Won);
    }
}
