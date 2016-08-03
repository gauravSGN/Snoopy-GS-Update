﻿using UnityEngine;
using System.Collections.Generic;
using Service;

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
    private GameObject currentAnimation;
    private Vector2 direction;
    private bool inputAllowed;

    public void CycleQueue()
    {
        if (!aimLine.Aiming && inputAllowed)
        {
            CycleLocalQueue();
            level.levelState.bubbleQueue.Rotate(nextBubbles.Length);
        }
    }

    public void AddShotModifier(ModifyShot modifier)
    {
        shotModifiers.Add(modifier);
        SetAimLineColor();
    }

    public void SetModifierAnimation(GameObject bubbleAnimation)
    {
        currentAnimation = bubbleAnimation;
        currentAnimation.transform.parent = nextBubbles[0].transform;
    }

    protected void Start()
    {
        nextBubbles = new GameObject[locations.Length];
        nextTypes = new BubbleType[locations.Length];
        shotModifiers = ResetShotModifiers();

        aimLine.Fire += FireBubbleAt;

        var eventService = GlobalState.Instance.Services.Get<EventService>();
        eventService.AddEventHandler<BubbleSettlingEvent>(OnBubbleSettleEvent);
        eventService.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
        eventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
        eventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
        inputAllowed = true;
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
        for (var index = 0; index < nextBubbles.Length; index++)
        {
            if (nextBubbles[index] == null)
            {
                nextTypes[index] = level.levelState.bubbleQueue.Peek(index);
                nextBubbles[index] = level.bubbleFactory.CreateByType(nextTypes[index]);
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
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new InputToggleEvent(false));
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new BubbleFiringEvent());
    }

    private void OnAnimationFireBubble()
    {
        nextBubbles[0].transform.parent = null;
        var rigidBody = nextBubbles[0].GetComponent<Rigidbody2D>();

        rigidBody.velocity = direction;
        rigidBody.gravityScale = 0.0f;
        rigidBody.isKinematic = false;

        GlobalState.Instance.Services.Get<EventService>().Dispatch(new BubbleFiredEvent(nextBubbles[0]));

        nextBubbles[0] = null;
        currentAnimation = null;
        direction = Vector2.zero;

        shotModifiers = ResetShotModifiers();
        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);
        level.levelState.bubbleQueue.GetNext();
    }

    private void ReadyNextBubble()
    {
        level.levelState.bubbleQueue.AddListener(OnBubbleQueueChanged);
        CycleLocalQueue();
        OnBubbleQueueChanged((Observable)level.levelState.bubbleQueue);
        characterController.OnAnimationFire -= OnAnimationFireBubble;
    }

    private void MoveBubbleToLocation(int index)
    {
        nextBubbles[index].transform.parent = locations[index].transform;
        nextBubbles[index].transform.localPosition = Vector3.zero;
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
            currentAnimation.transform.parent = nextBubbles[0].transform;
        }

        SetAimLineColor();
    }

    private void SetAimLineColor()
    {
        var color = nextBubbles[0].GetComponent<BubbleAttachments>().Model.definition.BaseColor;

        // If we have anything more than the default modifier, change the aimline to white.
        if (shotModifiers.Count > 1)
        {
            color = Color.white;
        }

        aimLine.Color = color;
    }

    private void OnBubbleSettleEvent(BubbleSettlingEvent gameEvent)
    {
        ReadyNextBubble();
    }

    private void OnReadyForNextBubbleEvent(ReadyForNextBubbleEvent gameEvent)
    {
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new InputToggleEvent(true));
    }

    private List<ModifyShot> ResetShotModifiers()
    {
        return new List<ModifyShot>{ AddBubbleSnap };
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
}
