using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Model;

public class BubbleRandomizerTests
{
    private ChainedRandomizer<int> singleOnce;
    private ChainedRandomizer<int> singleEach;

    private ChainedRandomizer<int> oneGenerator;
    private ChainedRandomizer<int> twoGenerator;
    private ChainedRandomizer<int> threeGenerator;
    private ChainedRandomizer<int> fourGenerator;

    private int[] items = { 1, 2, 3, 4 };

    [SetUp]
    public void Init()
    {
        singleOnce = new ChainedRandomizer<int>(ChainedRandomizer<int>.SelectionMethod.Once, items, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        singleEach = new ChainedRandomizer<int>(ChainedRandomizer<int>.SelectionMethod.Each, items, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });

        oneGenerator = new ChainedRandomizer<int>(ChainedRandomizer<int>.SelectionMethod.Once, items, new float[] { 1.0f, 0.0f, 0.0f, 0.0f });
        twoGenerator = new ChainedRandomizer<int>(ChainedRandomizer<int>.SelectionMethod.Once, items, new float[] { 0.0f, 2.0f, 0.0f, 0.0f });
        threeGenerator = new ChainedRandomizer<int>(ChainedRandomizer<int>.SelectionMethod.Once, items, new float[] { 0.0f, 0.0f, 3.0f, 0.0f });
        fourGenerator = new ChainedRandomizer<int>(ChainedRandomizer<int>.SelectionMethod.Once, items, new float[] { 0.0f, 0.0f, 0.0f, 4.0f });
    }

    [Test]
    public void OnlyValidTypesAreReturned()
    {
        Assert.AreEqual(1, oneGenerator.GetValue());
        Assert.AreEqual(2, twoGenerator.GetValue());
        Assert.AreEqual(3, threeGenerator.GetValue());
        Assert.AreEqual(4, fourGenerator.GetValue());
    }

    [Test]
    public void OnceMethodAlwaysReturnsSameValue()
    {
        var firstValue = singleOnce.GetValue();
        var iterations = 1000;

        while (iterations-- > 0)
        {
            Assert.AreEqual(firstValue, singleOnce.GetValue());
        }
    }

    [Test]
    public void EachMethodGeneratesAllValues()
    {
        var iterations = 1000;
        var generated = new bool[4];

        while (iterations-- > 0)
        {
            generated[singleEach.GetValue() - 1] = true;
        }

        Assert.IsTrue(generated[0]);
        Assert.IsTrue(generated[1]);
        Assert.IsTrue(generated[2]);
        Assert.IsTrue(generated[3]);
    }

    [Test]
    public void ExcludingValuesPreventsThemFromBeingGenerated()
    {
        singleEach.AddExclusion(oneGenerator);
        singleEach.AddExclusion(twoGenerator);
        singleEach.AddExclusion(fourGenerator);

        Assert.AreEqual(3, singleEach.GetValue());
    }

    [Test]
    public void OnlyOnceTypesCanBeUsedAsExclusions()
    {
        Assert.Throws<System.ArgumentException>(() => singleOnce.AddExclusion(singleEach));
    }
}
