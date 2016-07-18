using System.Collections.Generic;
using System.Linq;

abstract public class BaseBubbleQueue : Observable, BubbleQueue
{
    public const int MAX_QUEUE_SIZE = 4;

    public static readonly BubbleType[] LAUNCHER_BUBBLE_TYPES =
    {
        BubbleType.Blue,
        BubbleType.Yellow,
        BubbleType.Red,
        BubbleType.Green,
        BubbleType.Pink,
        BubbleType.Purple,
    };

    protected readonly LevelState levelState;
    protected readonly List<BubbleType> queued = new List<BubbleType>();

    abstract protected BubbleType GenerateElement();
    abstract protected void RemoveInactiveTypes();

    public BaseBubbleQueue(LevelState state)
    {
        levelState = state;
        state.AddListener(OnLevelStateChanged);

        foreach(var type in LAUNCHER_BUBBLE_TYPES.Where(bubbleType => !levelState.typeTotals.ContainsKey(bubbleType)))
        {
            levelState.typeTotals[type] = 0;
        }
    }

    public BubbleType GetNext()
    {
        var next = queued[0];

        queued.RemoveAt(0);
        BuildQueue();

        return next;
    }

    public BubbleType Peek(int index)
    {
        return queued[index];
    }

    public void Rotate(int count)
    {
        var first = queued[0];

        queued.RemoveAt(0);
        queued.Insert(count - 1, first);
    }

    protected void BuildQueue()
    {
        var modified = queued.Count < MAX_QUEUE_SIZE;

        while (queued.Count < MAX_QUEUE_SIZE)
        {
            queued.Add(GenerateElement());
        }

        if (modified)
        {
            NotifyListeners();
        }
    }

    private void OnLevelStateChanged(Observable state)
    {
        RemoveInactiveTypes();
        BuildQueue();
    }
}