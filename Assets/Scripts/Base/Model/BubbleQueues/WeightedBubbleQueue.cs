using System.Collections.Generic;
using System.Linq;
using System;

public class WeightedBubbleQueue : BaseBubbleQueue, BubbleQueue
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

    private readonly Dictionary<BubbleType, OptionData> options = new Dictionary<BubbleType, OptionData>();
    private readonly Random rng = new Random();
    private int counter = 0;

    public int ExtrasCount { get { return 10; } }

    public WeightedBubbleQueue(LevelState state) : base(state)
    {
        foreach (var type in LAUNCHER_BUBBLE_TYPES)
        {
            options.Add(type, new OptionData());
        }

        RemoveInactiveTypes();
        BuildQueue();
    }

    public void SwitchToExtras()
    {
        // Nothing to do for this queue type
    }

    override protected BubbleType GenerateElement()
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

    override protected void RemoveInactiveTypes()
    {
        var typeTotals = levelState.typeTotals;
        var keys = new List<BubbleType>(options.Keys);

        for (var index = 0; index < keys.Count; index++)
        {
            var type = keys[index];

            if (!typeTotals.ContainsKey(type) || (typeTotals[type] <= 0))
            {
                options.Remove(type);
            }
        }

        queued.RemoveAll(i => !options.ContainsKey(i));
    }
}
