using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelState : Observable
{
    private const string SLIDEOUT_PATH = "Slideouts/LowMovesSlideout";

    public int score;
    public int levelNumber;
    public int[] starValues;
    public int initialShotCount;
    public int remainingBubbles;
    public BubbleQueue bubbleQueue;
    public Dictionary<BubbleType, int> typeTotals = new Dictionary<BubbleType, int>();
    public Dictionary<BubbleType, int> initialTypeTotals = new Dictionary<BubbleType, int>();
    public bool preparedForBubbleParty;

    private GameObject slideout;
    private bool loadingSlideout;

    private bool AtLowMovesThreshold
    {
        get { return remainingBubbles == GlobalState.Instance.Config.level.lowMovesThreshold; }
    }

    public void UpdateTypeTotals(BubbleType type, int delta)
    {
        if (!initialTypeTotals.ContainsKey(type))
        {
            initialTypeTotals[type] = delta;
        }

        typeTotals[type] = typeTotals.ContainsKey(type) ? typeTotals[type] + delta : delta;
        NotifyListeners();
    }

    public void DecrementRemainingBubbles()
    {
        remainingBubbles--;
        NotifyListeners();

        var eventService = GlobalState.EventService;

        // Need to start loading this early so it's ready when we need it.  It also needs to start loading at a point
        // where we can be sure that the asset service is available.
        if ((slideout == null) && !loadingSlideout)
        {
            loadingSlideout = true;

            GlobalState.AssetService.LoadAssetAsync<GameObject>(SLIDEOUT_PATH, (s) =>
            {
                slideout = s;
                loadingSlideout = false;
            });

            eventService.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubble);
        }

        eventService.Dispatch(new ShotsRemainingEvent(remainingBubbles));

        if (!preparedForBubbleParty && AtLowMovesThreshold)
        {
            eventService.Dispatch(new LowMovesEvent(remainingBubbles));
        }
    }

    public void AddRemainingBubbles(int count)
    {
        remainingBubbles += count;
        NotifyListeners();
    }

    private IEnumerator ShowLowMovesSlideout()
    {
        while (slideout == null)
        {
            yield return null;
        }

        GlobalState.EventService.Dispatch(new Slideout.ShowSlideoutEvent(slideout));
    }

    private void OnReadyForNextBubble()
    {
        if (AtLowMovesThreshold)
        {
            GlobalState.Instance.RunCoroutine(ShowLowMovesSlideout());
        }
    }
}
