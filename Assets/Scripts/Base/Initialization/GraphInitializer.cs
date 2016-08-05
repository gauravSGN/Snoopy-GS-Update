using Service;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using DependencyDict = System.Collections.Generic.Dictionary<Initializable, Initializable[]>;

public class GraphInitializer : MonoBehaviour, InitializerService
{
    private readonly DependencyDict toInitialize = new DependencyDict();
    private GlobalState state;

    public void Awake()
    {
        state = GetComponent<GlobalState>();
        state.Services.SetInstance<InitializerService>(this);
    }

    public void Register(Initializable target, params Initializable[] dependencies)
    {
        var first = toInitialize.Count == 0;

        InsertEntry(target);
        toInitialize[target] = dependencies;

        foreach (var dependency in dependencies)
        {
            InsertEntry(dependency);
        }

        if (first)
        {
            state.RunCoroutine(InitializeAll());
        }
    }

    private void InsertEntry(Initializable entry)
    {
        if ((entry != null) && !toInitialize.ContainsKey(entry))
        {
            toInitialize.Add(entry, null);
        }
    }

    private IEnumerator InitializeAll()
    {
        yield return null;

        var initOrder = new List<Initializable>();

        foreach (var pair in toInitialize)
        {
            FlattenNode(initOrder, pair.Key);
        }

        initOrder.ForEach(n => n.PreInitialize());
        initOrder.ForEach(n => n.Initialize());
        initOrder.ForEach(n => n.PostInitialize());

        toInitialize.Clear();
    }

    private void FlattenNode(List<Initializable> initOrder, Initializable node)
    {
        if (!initOrder.Contains(node))
        {
            var dependencies = toInitialize[node];
            var index = initOrder.Count;
            initOrder.Add(node);

            foreach (var dependency in dependencies)
            {
                FlattenNode(initOrder, dependency);
            }

            initOrder.Add(node);
            initOrder.RemoveAt(index);
        }
    }
}
