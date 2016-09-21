using Sound;
using Sequence;
using UnityEngine;

public class BubbleLauncher : MonoBehaviour
{
    [SerializeField]
    private Level level;

    [SerializeField]
    private AimLine aimLine;

    [SerializeField]
    private LauncherCharacterController characterController;

    [SerializeField]
    private InputStatus input;

    private Vector2 direction;
    private AudioClip launchSoundOverride;
    private GameObject shooterBubble;

    public void Start()
    {
        aimLine.Fire += FireBubbleAt;

        var eventService = GlobalState.EventService;
        eventService.AddEventHandler<SetShooterBubbleEvent>(OnSetShooterBubble);
        eventService.AddEventHandler<BubbleSettlingEvent>(OnBubbleSettleEvent);
        eventService.AddEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);

        Util.FrameUtil.AtEndOfFrame(() =>
        {
            var soundService = GlobalState.SoundService;
            soundService.PreloadSound(SoundType.LaunchBubble);
            soundService.PreloadSound(SoundType.SwapBubbles);
            soundService.PreloadSound(SoundType.FailSwapBubbles);
        });
    }

    public void SetLaunchSoundOverride(AudioClip overrideClip)
    {
        launchSoundOverride = overrideClip;
    }

    private void FireBubbleAt(Vector2 point)
    {
        GlobalState.EventService.Dispatch(new InputToggleEvent(false));

        shooterBubble.AddComponent<BubbleSnap>();

        var launchSpeed = GlobalState.Instance.Config.aimline.launchSpeed;
        direction = (point - (Vector2)shooterBubble.transform.position).normalized * launchSpeed;
        characterController.OnAnimationFire += OnAnimationFireBubble;

        GlobalState.EventService.Dispatch(new BubbleFiringEvent());
    }

    private void OnAnimationFireBubble()
    {
        shooterBubble.transform.parent = null;
        var rigidBody = shooterBubble.GetComponent<Rigidbody2D>();

        rigidBody.velocity = direction;
        rigidBody.gravityScale = 0.0f;
        rigidBody.isKinematic = false;

        GlobalState.EventService.Dispatch(new BubbleFiredEvent(shooterBubble));

        if (launchSoundOverride != null)
        {
            PlaySoundEvent.Dispatch(launchSoundOverride);
        }
        else
        {
            PlaySoundEvent.Dispatch(SoundType.LaunchBubble);
        }
    }

    private void OnSetShooterBubble(SetShooterBubbleEvent gameEvent)
    {
        shooterBubble = gameEvent.bubble;
    }

    private void OnBubbleSettleEvent()
    {
        launchSoundOverride = null;
        characterController.OnAnimationFire -= OnAnimationFireBubble;
    }

    private void OnPrepareForBubbleParty()
    {
        var eventService = GlobalState.EventService;
        eventService.RemoveEventHandler<SetShooterBubbleEvent>(OnSetShooterBubble);
        eventService.RemoveEventHandler<BubbleSettlingEvent>(OnBubbleSettleEvent);
        eventService.RemoveEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
    }
}
