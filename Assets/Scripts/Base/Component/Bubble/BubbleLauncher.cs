using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BubbleLauncher : MonoBehaviour
{
    public delegate void ModifyShot(GameObject bubble);

    [SerializeField]
    private GameObject[] locations;

    [SerializeField]
    private Level level;

    [SerializeField]
    private AimLine aimLine;

    [SerializeField]
    private LauncherCharacterController characterController;

    [SerializeField]
    private AudioClip launchSound;

    [SerializeField]
    private AudioClip swapSound;

    [SerializeField]
    private AudioClip swapFailSound;

    [SerializeField]
    private GameObject shooterTrailPrefab;

    private GameObject[] nextBubbles;
    private BubbleType[] nextTypes;
    private List<ModifyShot> shotModifiers;
    private HashSet<ShotModifierType> shotModifierTypes;
    private GameObject currentAnimation;
    private Vector2 direction;
    private bool inputAllowed;
    private AudioSource audioSource;
    private AudioClip launchSoundOverride;
    private GameObject shooterTrail;

    public void CycleQueue()
    {
        if (!aimLine.Aiming && inputAllowed)
        {
            GlobalState.EventService.Dispatch(new InputToggleEvent(false));
            CycleLocalQueue();
            level.levelState.bubbleQueue.Rotate(nextBubbles.Length);
            SetAimLineColor();
            characterController.CycleQueueAnimation();

            if (nextBubbles.Length > 1)
            {
                PlaySound(swapSound);
            }
            else
            {
                PlaySound(swapFailSound);
            }
        }
    }

    public void AddShotModifier(ModifyShot modifier, ShotModifierType type)
    {
        shotModifiers.Add(modifier);
        shotModifierTypes.Add(type);

        SetAimLineColor();

        GlobalState.EventService.Dispatch(new AddShotModifierEvent(type));
    }

    public void SetModifierAnimation(GameObject bubbleAnimation)
    {
        currentAnimation = bubbleAnimation;
        UpdateCurrentAnimationTransform();
    }

    public void SetLaunchSoundOverride(AudioClip overrideClip)
    {
        launchSoundOverride = overrideClip;
    }

    protected void Start()
    {
        nextBubbles = new GameObject[locations.Length];
        nextTypes = new BubbleType[locations.Length];
        shooterTrail = Instantiate(shooterTrailPrefab);
        shooterTrail.transform.SetParent(transform, false);

        ResetShotModifiers();

        aimLine.Fire += FireBubbleAt;

        var eventService = GlobalState.EventService;
        eventService.AddEventHandler<BubbleSettlingEvent>(OnBubbleSettleEvent);
        eventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
        eventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
        eventService.AddEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
        eventService.AddEventHandler<PurchasedExtraMovesEvent>(OnPurchasedExtraMoves);

        inputAllowed = true;

        audioSource = GetComponent<AudioSource>();
    }

    private void OnPrepareForBubbleParty(PrepareForBubblePartyEvent gameEvent)
    {
        var eventService = GlobalState.EventService;
        eventService.RemoveEventHandler<BubbleSettlingEvent>(OnBubbleSettleEvent);
        eventService.RemoveEventHandler<LevelLoadedEvent>(OnLevelLoaded);
        eventService.RemoveEventHandler<InputToggleEvent>(OnInputToggle);
        eventService.RemoveEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
        eventService.RemoveEventHandler<PurchasedExtraMovesEvent>(OnPurchasedExtraMoves);
        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);

        foreach (var bubble in nextBubbles)
        {
            Destroy(bubble);
        }
    }

    private void OnLevelLoaded(LevelLoadedEvent gameEvent)
    {
        CreateBubbles();
        SetAimLineColor();
        level.levelState.bubbleQueue.AddListener(OnBubbleQueueChanged);
    }

    protected void OnDestroy()
    {
        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);
    }

    private void CreateBubbles()
    {
        var offset = new Vector3(0, -GlobalState.Instance.Config.bubbles.size / 2);

        for (var index = 0; index < nextBubbles.Length; index++)
        {
            if (nextBubbles[index] == null)
            {
                nextTypes[index] = level.levelState.bubbleQueue.Peek(index);
                nextBubbles[index] = level.bubbleFactory.CreateByType(nextTypes[index]);

                nextBubbles[index].transform.parent = locations[index].transform;
                nextBubbles[index].transform.position = locations[index].transform.position + offset;
                nextBubbles[index].layer = (int)Layers.Default;

                MoveBubbleToLocation(index);
            }
        }
    }

    private void FireBubbleAt(Vector2 point)
    {
        GlobalState.EventService.Dispatch(new InputToggleEvent(false));

        foreach (var modifier in shotModifiers)
        {
            modifier(nextBubbles[0]);
        }

        var launchSpeed = GlobalState.Instance.Config.aimline.launchSpeed;
        direction = (point - (Vector2)locations[0].transform.position).normalized * launchSpeed;
        characterController.OnAnimationFire += OnAnimationFireBubble;
        GlobalState.EventService.Dispatch(new BubbleFiringEvent());
    }

    private void OnAnimationFireBubble()
    {
        nextBubbles[0].transform.parent = null;
        var rigidBody = nextBubbles[0].GetComponent<Rigidbody2D>();

        rigidBody.velocity = direction;
        rigidBody.gravityScale = 0.0f;
        rigidBody.isKinematic = false;

        shooterTrail.transform.SetParent(nextBubbles[0].transform, false);
        shooterTrail.transform.localPosition = Vector3.forward;
        shooterTrail.SetActive(true);

        GlobalState.EventService.Dispatch(new BubbleFiredEvent(nextBubbles[0]));

        nextBubbles[0] = null;
        currentAnimation = null;
        direction = Vector2.zero;

        ResetShotModifiers();

        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);
        level.levelState.bubbleQueue.GetNext();

        PlaySound(launchSoundOverride ?? launchSound);
    }

    private void ReadyNextBubble()
    {
        launchSoundOverride = null;
        level.levelState.bubbleQueue.AddListener(OnBubbleQueueChanged);
        CycleLocalQueue();

        if ((level.levelState.remainingBubbles < nextBubbles.Length) && (nextBubbles.Length != 0))
        {
            Array.Resize(ref nextBubbles, Mathf.Max(0, level.levelState.remainingBubbles));
            Array.Resize(ref nextTypes, Mathf.Max(0, level.levelState.remainingBubbles));
        }

        OnBubbleQueueChanged((Observable)level.levelState.bubbleQueue);
        characterController.OnAnimationFire -= OnAnimationFireBubble;
        SetAimLineColor();
    }

    private void MoveBubbleToLocation(int index)
    {
        nextBubbles[index].transform.parent = locations[index].transform;
        StartCoroutine(MoveBubbleRoutine(nextBubbles[index].transform, locations[index].transform));
    }

    public IEnumerator MoveBubbleRoutine(Transform bubbleTransform, Transform target)
    {
        if (bubbleTransform != null)
        {
            float time = 0f;
            float totalTime = 0.15f;
            Vector3 start = new Vector3(bubbleTransform.position.x, bubbleTransform.position.y, -1f);
            Vector3 end = new Vector3(target.position.x, target.position.y, -1f);

            while (time <= totalTime)
            {
                if (bubbleTransform != null)
                {
                    time += Time.deltaTime;
                    bubbleTransform.position = Vector3.Slerp(start, end, (time/totalTime));
                    yield return null;
                }
                else
                {
                    break;
                }
            }

            if (bubbleTransform != null)
            {
                bubbleTransform.localPosition = Vector3.zero;
            }
        }
    }

    private void CycleLocalQueue()
    {
        var lastIndex = nextBubbles.Length - 1;
        var first = nextBubbles[0];
        var firstType = nextTypes[0];

        for (var index = 0; index < lastIndex; index++)
        {
            nextBubbles[index] = nextBubbles[index + 1];
            nextTypes[index] = nextTypes[index + 1];

            if (nextBubbles[index] != null)
            {
                MoveBubbleToLocation(index);
            }
        }

        nextTypes[lastIndex] = firstType;
        nextBubbles[lastIndex] = first;

        if (first != null)
        {
            MoveBubbleToLocation(lastIndex);
        }

        if (currentAnimation != null)
        {
            UpdateCurrentAnimationTransform();
        }
    }

    private void SetAimLineColor()
    {
        if (nextBubbles.Length > 0)
        {
            var colors = new[] { nextBubbles[0].GetComponent<BubbleModelBehaviour>().Model.definition.BaseColor };

            if (shotModifierTypes.Count > 0)
            {
                if (shotModifierTypes.Contains(ShotModifierType.RainbowBooster))
                {
                    colors = GlobalState.Instance.Config.boosters.rainbowColors;
                }
                else
                {
                    colors[0] = Color.white;
                }
            }

            aimLine.colors = colors;
        }
    }

    private void OnBubbleSettleEvent(BubbleSettlingEvent gameEvent)
    {
        shooterTrail.SetActive(false);
        shooterTrail.transform.SetParent(transform, false);

        ReadyNextBubble();
    }

    private void ResetShotModifiers()
    {
        shotModifiers = new List<ModifyShot> { AddBubbleSnap };
        shotModifierTypes = new HashSet<ShotModifierType>();
    }

    private void AddBubbleSnap(GameObject bubble)
    {
        bubble.AddComponent<BubbleSnap>();
    }

    private void OnBubbleQueueChanged(Observable target)
    {
        var bubbleQueue = target as BubbleQueue;

        for (var index = 0; index < nextTypes.Length; index++)
        {
            if ((nextTypes[index] != bubbleQueue.Peek(index)) && (nextBubbles[index] != null))
            {
                Destroy(nextBubbles[index]);
                nextBubbles[index] = null;
            }
        }

        CreateBubbles();
        SetAimLineColor();
    }

    private void OnPurchasedExtraMoves(PurchasedExtraMovesEvent gameEvent)
    {
        var queue = level.levelState.bubbleQueue;

        queue.SwitchToExtras();
        level.levelState.AddRemainingBubbles(10);

        nextBubbles = new GameObject[locations.Length];
        nextTypes = new BubbleType[locations.Length];
        CreateBubbles();
        SetAimLineColor();

        GlobalState.EventService.Dispatch(new InputToggleEvent(true));
    }

    private void OnInputToggle(InputToggleEvent gameEvent)
    {
        inputAllowed = gameEvent.enabled;
    }

    private void UpdateCurrentAnimationTransform()
    {
        currentAnimation.transform.SetParent(nextBubbles[0].transform);
        currentAnimation.transform.localPosition = Vector3.back;
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
