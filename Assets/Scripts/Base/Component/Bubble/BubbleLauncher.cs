using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Note: Enabling input happens within the Launcher Character's state machine to
// account for animation and transition times.
public class BubbleLauncher : MonoBehaviour
{
    public delegate void ModifyShot(GameObject bubble);

    [SerializeField]
    private GameObject[] locations;

    [SerializeField]
    private float launchSpeed;

    [SerializeField]
    private Level level;

    [SerializeField]
    private AimLine aimLine;

    [SerializeField]
    private LauncherCharacterController characterController;

    private GameObject[] nextBubbles;
    private BubbleType[] nextTypes;
    private List<ModifyShot> shotModifiers;
    private HashSet<ShotModifierType> shotModifierTypes;
    private GameObject currentAnimation;
    private Vector2 direction;
    private bool inputAllowed;
    private AudioSource launchSound;

    public void CycleQueue()
    {
        if (!aimLine.Aiming && inputAllowed)
        {
            GlobalState.EventService.Dispatch(new InputToggleEvent(false));
            CycleLocalQueue();
            level.levelState.bubbleQueue.Rotate(nextBubbles.Length);
            SetAimLineColor();
            characterController.CycleQueueAnimation();
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

    protected void Start()
    {
        nextBubbles = new GameObject[locations.Length];
        nextTypes = new BubbleType[locations.Length];

        ResetShotModifiers();

        aimLine.Fire += FireBubbleAt;

        var eventService = GlobalState.EventService;

        eventService.AddEventHandler<BubbleSettlingEvent>(OnBubbleSettleEvent);
        eventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
        eventService.AddEventHandler<InputToggleEvent>(OnInputToggle);

        inputAllowed = true;

        launchSound = GetComponent<AudioSource>();
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
        foreach (var modifier in shotModifiers)
        {
            modifier(nextBubbles[0]);
        }

        direction = (point - (Vector2)locations[0].transform.position).normalized * launchSpeed;
        characterController.OnAnimationFire += OnAnimationFireBubble;
        GlobalState.EventService.Dispatch(new InputToggleEvent(false));
        GlobalState.EventService.Dispatch(new BubbleFiringEvent());
    }

    private void OnAnimationFireBubble()
    {
        nextBubbles[0].transform.parent = null;
        var rigidBody = nextBubbles[0].GetComponent<Rigidbody2D>();

        rigidBody.velocity = direction;
        rigidBody.gravityScale = 0.0f;
        rigidBody.isKinematic = false;

        GlobalState.EventService.Dispatch(new BubbleFiredEvent(nextBubbles[0]));

        nextBubbles[0] = null;
        currentAnimation = null;
        direction = Vector2.zero;

        ResetShotModifiers();

        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);
        level.levelState.bubbleQueue.GetNext();

        launchSound.Play();
    }

    private void ReadyNextBubble()
    {
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
            var colors = new Color[] { nextBubbles[0].GetComponent<BubbleAttachments>().Model.definition.BaseColor };

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

    private void OnInputToggle(InputToggleEvent gameEvent)
    {
        inputAllowed = gameEvent.enabled;
    }

    private void UpdateCurrentAnimationTransform()
    {
        currentAnimation.transform.SetParent(nextBubbles[0].transform);
        currentAnimation.transform.localPosition = Vector3.back;
    }
}
