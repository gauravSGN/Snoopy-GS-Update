using Spine;
using System;
using UnityEngine;
using Spine.Unity;

public class LauncherCharacterController : MonoBehaviour
{
    private const string ANGLE = "Angle";
    private const string AIMING = "Aiming";
    private const string FIRING = "Firing";
    private const string LOSING_LEVEL = "LosingLevel";
    private const string LOST_LEVEL = "LostLevel";
    private const string WON_LEVEL = "WonLevel";
    private const string SWAP = "Swap";

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

    public void CycleQueueAnimation()
    {
        launcherAnimator.SetTrigger(SWAP);
    }

    public void StartBubbleParty()
    {
        Sequence.StartBubblePartyEvent.Dispatch();
    }

    protected void Start()
    {
        launcherAnimator = gameObject.GetComponent<Animator>();
        skeleton = gameObject.GetComponent<SkeletonAnimator>().Skeleton;

        launcherAnimator.SetFloat(ANGLE, 90.0f);
        eventTrigger.StartAiming += OnStartAiming;
        eventTrigger.StopAiming += OnStopAiming;
        eventTrigger.MoveTarget += OnMoveTarget;

        var eventService = GlobalState.EventService;
        eventService.AddEventHandler<BubbleFiringEvent>(OnBubbleFiring);
        eventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        eventService.AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
        eventService.AddEventHandler<LowMovesEvent>(OnLowMoves);
        eventService.AddEventHandler<PurchasedExtraMovesEvent>(OnPurchasedExtraMoves);
    }

    private void OnBubbleFiring()
    {
        launcherAnimator.SetBool(AIMING, false);
        launcherAnimator.SetTrigger(FIRING);
    }

    private void OnBubbleFired()
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

    private void OnLowMoves()
    {
        launcherAnimator.SetBool(LOSING_LEVEL, true);
    }

    private void OnLevelComplete(LevelCompleteEvent gameEvent)
    {
        launcherAnimator.SetBool(WON_LEVEL, gameEvent.Won);
        launcherAnimator.SetBool(LOST_LEVEL, !gameEvent.Won);
    }

    private void OnPurchasedExtraMoves()
    {
        launcherAnimator.SetBool(LOST_LEVEL, false);
    }
}
