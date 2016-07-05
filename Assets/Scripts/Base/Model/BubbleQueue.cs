using System.Collections.Generic;
using System.Linq;
using System;

public class BubbleQueue : Observable
{
    private class OptionData
    {
        public float weight = 1.0f;
        public int lastPicked;

        public float RollWeight(int counter)
        {
            return weight * Math.Max(1, counter - lastPicked);
        }
    }

    public const int MAX_QUEUE_SIZE = 4;
    public static readonly BubbleType[] LAUNCHER_BUBBLE_TYPES =
    {
        BubbleType.Blue,
        BubbleType.Yellow,
        BubbleType.Red,
        BubbleType.Green,
    };

    private readonly Dictionary<BubbleType, OptionData> options = new Dictionary<BubbleType, OptionData>();
    private readonly List<BubbleType> queued = new List<BubbleType>();
    private readonly LevelState levelState;
    private readonly Random rng = new Random();
    private int counter = 0;

    public BubbleQueue(LevelState state)
    {
        foreach (var type in LAUNCHER_BUBBLE_TYPES)
        {
            options.Add(type, new OptionData());
        }

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

    private BubbleType GenerateElement()
    {
        var totalWeight = options.Sum(o => o.Value.RollWeight(counter));
        var result = (float)rng.NextDouble() * totalWeight;

        counter++;

        foreach (var pair in options)
        {
            var weight = pair.Value.RollWeight(counter);

            if (result < weight)
            {
                pair.Value.lastPicked = counter;
                return pair.Key;
            }

            result -= weight;
        }

        return BubbleType.Blue;
    }

    private void BuildQueue()
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

    private void RemoveInactiveTypes()
    {
        var typeTotals = levelState.typeTotals;
        var keys = new List<BubbleType>(options.Keys);
        var count = keys.Count;

        for (var index = 0; index < keys.Count; index++)
        {
            var type = keys[index];

            if (!typeTotals.ContainsKey(type) || (typeTotals[type] <= 0))
            {
                options.Remove(type);
            }
        }

        for (var index = 0; index < queued.Count; index++)
        {
            if (!options.ContainsKey(queued[index]))
            {
                queued.RemoveAt(index);
                index--;
            }
        }
    }

    private void OnLevelStateChanged(Observable state)
    {
        RemoveInactiveTypes();
        BuildQueue();
    }
}
