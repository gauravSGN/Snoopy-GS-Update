using Sound;
using Aiming;
using System;
using Sequence;
using UnityEngine;
using System.Collections;

sealed public class BubbleDispenser : MonoBehaviour
{
    [SerializeField]
    private Transform[] locations;

    [SerializeField]
    private Level level;

    [SerializeField]
    private InputStatus input;

    private GameObject[] nextBubbles;
    private BubbleType[] nextTypes;
    private bool aiming;

    public void Start()
    {
        var eventService = GlobalState.EventService;
        eventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
        eventService.AddEventHandler<StartAimingEvent>(() => { aiming = true; });
        eventService.AddEventHandler<StopAimingEvent>(() => { aiming = false; });
        eventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        eventService.AddEventHandler<BubbleSettlingEvent>(OnBubbleSettling);
        eventService.AddEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
        eventService.AddEventHandler<PurchasedExtraMovesEvent>(OnPurchasedExtraMoves);
    }

    public void OnDestroy()
    {
        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);
    }

    public void Cycle()
    {
        if (!aiming && input.Enabled)
        {
            GlobalState.EventService.Dispatch(new InputToggleEvent(false));
            CycleLocalQueue();
            level.levelState.bubbleQueue.Rotate(nextBubbles.Length);
            SwapBubblesEvent.Dispatch();

            PlaySoundEvent.Dispatch((nextBubbles.Length > 1) ? SoundType.SwapBubbles : SoundType.FailSwapBubbles);
        }
    }

    private void Refill()
    {
        nextBubbles = new GameObject[locations.Length];
        nextTypes = new BubbleType[locations.Length];

        CreateBubbles();
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

                nextBubbles[index].transform.SetParent(locations[index], false);
                nextBubbles[index].transform.position = locations[index].position + offset;
                nextBubbles[index].layer = (int)Layers.Default;

                StartCoroutine(MoveBubbleRoutine(nextBubbles[index].transform, locations[index], 0.0f));
            }
        }

        SetShooterBubbleEvent.Dispatch((nextBubbles.Length > 0) ? nextBubbles[0] : null);
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
        }

        nextTypes[lastIndex] = firstType;
        nextBubbles[lastIndex] = first;

        for (var index = 0; index <= lastIndex; index++)
        {
            if (nextBubbles[index] != null)
            {
                MoveBubbleWithArc(index);
            }
        }

        SetShooterBubbleEvent.Dispatch(nextBubbles[0]);
    }

    private void MoveBubbleWithArc(int index)
    {
        nextBubbles[index].transform.SetParent(locations[index], true);
        StartCoroutine(MoveBubbleRoutine(nextBubbles[index].transform, locations[index], 0.25f));
    }

    public IEnumerator MoveBubbleRoutine(Transform bubbleTransform, Transform target, float arcOffset)
    {
        if (bubbleTransform != null)
        {
            var time = 0f;
            var totalTime = 0.15f;
            var start = new Vector3(bubbleTransform.position.x, bubbleTransform.position.y);
            var end = new Vector3(target.position.x, target.position.y);
            var midpoint = ((start + end) / 2.0f) + (Vector3.Cross(end - start, Vector3.forward) * arcOffset);

            start -= midpoint;
            end -= midpoint;
            midpoint += Vector3.back;

            while ((bubbleTransform != null) && (time <= totalTime))
            {
                time += Time.deltaTime;
                bubbleTransform.position = midpoint + Vector3.Slerp(start, end, (time / totalTime));
                yield return null;
            }

            if (bubbleTransform != null)
            {
                bubbleTransform.localPosition = Vector3.zero;
            }
        }
    }

    private void OnLevelLoaded()
    {
        GlobalState.EventService.RemoveEventHandler<LevelLoadedEvent>(OnLevelLoaded);

        Refill();

        level.levelState.bubbleQueue.AddListener(OnBubbleQueueChanged);
    }

    private void OnBubbleFired()
    {
        nextBubbles[0] = null;
        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);
        level.levelState.bubbleQueue.GetNext();
    }

    private void OnBubbleSettling()
    {
        CycleLocalQueue();
        level.levelState.bubbleQueue.AddListener(OnBubbleQueueChanged);

        if ((level.levelState.remainingBubbles < nextBubbles.Length) && (nextBubbles.Length != 0))
        {
            Array.Resize(ref nextBubbles, Mathf.Max(0, level.levelState.remainingBubbles));
            Array.Resize(ref nextTypes, Mathf.Max(0, level.levelState.remainingBubbles));
        }

        OnBubbleQueueChanged((Observable)level.levelState.bubbleQueue);
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
    }

    private void OnPrepareForBubbleParty()
    {
        var eventService = GlobalState.EventService;
        eventService.RemoveEventHandler<BubbleSettlingEvent>(CycleLocalQueue);
        eventService.RemoveEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
        eventService.RemoveEventHandler<PurchasedExtraMovesEvent>(OnPurchasedExtraMoves);

        level.levelState.bubbleQueue.RemoveListener(OnBubbleQueueChanged);

        foreach (var bubble in nextBubbles)
        {
            Destroy(bubble);
        }
    }

    private void OnPurchasedExtraMoves()
    {
        level.levelState.bubbleQueue.SwitchToExtras();
        level.levelState.AddRemainingBubbles(10);

        Refill();

        GlobalState.EventService.Dispatch(new InputToggleEvent(true));
    }
}
