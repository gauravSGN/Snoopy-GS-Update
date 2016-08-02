using Model;
using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public class RandomBagTests
{
    [Test]
    public void ItemsCanBeAddedToBag()
    {
        var bag = new RandomBag<int>();
        var startCount = bag.Count;

        bag.Add(7);
        Assert.AreEqual(startCount + 1, bag.Count);

        bag.Add(4, 3);
        Assert.AreEqual(startCount + 4, bag.Count);
    }

    [Test]
    public void ItemsCanBeRemovedFromBag()
    {
        var bag = new RandomBag<int>(new[] { 1, 2, 3 });
        Assert.IsFalse(bag.Empty);

        bag.Next();
        bag.Next();
        bag.Next();

        Assert.IsTrue(bag.Empty);
        Assert.AreEqual(0, bag.Count);
    }

    [Test]
    public void BagCanBeCleared()
    {
        var bag = new RandomBag<int>(new[] { 1, 2, 3 });
        Assert.IsFalse(bag.Empty);

        bag.Clear();

        Assert.IsTrue(bag.Empty);
        Assert.AreEqual(0, bag.Count);
    }

    [Test]
    public void ItemsCannotBeRemovedWhenBagIsEmpty()
    {
        var bag = new RandomBag<int>();

        Assert.IsTrue(bag.Empty);
        Assert.Throws<InvalidOperationException>(() => bag.Next());
    }

    [Test]
    public void AllItemsInBagArePresentInOutput()
    {
        var items = GetRandomItems(20, 100);

        var bag = new RandomBag<int>(items);
        var results = new Dictionary<int, int>();

        while (!bag.Empty)
        {
            var value = bag.Next();

            if (!results.ContainsKey(value))
            {
                results.Add(value, 0);
            }

            results[value]++;
        }

        foreach (var item in items)
        {
            results[item]--;
        }

        Assert.IsFalse(results.Any(p => p.Value != 0));
    }

    [Test]
    public void ItemsComeFromBagInRandomOrder()
    {
        var items = GetRandomItems(50, 100);
        var bag = new RandomBag<int>(items);
        var results = items.Select(i => bag.Next()).ToArray();

        int differences = 0;
        for (var index = 0; index < items.Length; index++)
        {
            if (items[index] != results[index])
            {
                differences++;
            }
        }

        Assert.Greater(differences, 0);
    }

    private int[] GetRandomItems(int quantity, int maxValue)
    {
        var rng = new Random();
        var items = new int[quantity];

        for (var index = 0; index < items.Length; index++)
        {
            items[index] = rng.Next(maxValue);
        }

        return items;
    }
}
