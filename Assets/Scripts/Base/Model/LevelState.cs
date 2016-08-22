using System.Collections.Generic;

public class LevelState : Observable
{
    public int score;
    public int levelNumber;
    public int[] starValues;
    public int initialShotCount;
    public int remainingBubbles;
    public BubbleQueue bubbleQueue;
    public Dictionary<BubbleType, int> typeTotals = new Dictionary<BubbleType, int>();
    public Dictionary<BubbleType, int> initialTypeTotals = new Dictionary<BubbleType, int>();

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
        GlobalState.EventService.Dispatch(new ShotsRemainingEvent(remainingBubbles));
    }
}
