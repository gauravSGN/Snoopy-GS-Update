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
        }

        GlobalState.EventService.Dispatch(new ShotsRemainingEvent(remainingBubbles));

        if (!preparedForBubbleParty && (remainingBubbles == GlobalState.Instance.Config.level.lowMovesThreshold))
        {
            GlobalState.EventService.Dispatch(new LowMovesEvent(remainingBubbles));
            GlobalState.Instance.RunCoroutine(ShowLowMovesSlideout());
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
}
