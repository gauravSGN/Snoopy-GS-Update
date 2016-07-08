using System.Collections.Generic;

abstract public class BaseBubbleQueue : Observable
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
    abstract protected void BuildQueue();
    abstract protected void RemoveInactiveTypes();

    public BaseBubbleQueue(LevelState state)
    {
        levelState = state;
        state.AddListener(OnLevelStateChanged);
        BuildQueue();
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

    private void OnLevelStateChanged(Observable state)
    {
        RemoveInactiveTypes();
        BuildQueue();
    }
}