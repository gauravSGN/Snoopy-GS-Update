using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BubbleEffectController : MonoBehaviour, UpdateReceiver
{
    private readonly List<IEnumerator> effects = new List<IEnumerator>();

    public int Count { get { return effects.Count; } }

    public void OnDestroy()
    {
        GlobalState.Instance.UpdateDispatcher.Updates.Remove(this);
    }

    public void OnUpdate()
    {
        var index = 0;

        while (index < effects.Count)
        {
            var effect = effects[index];

            if (effect.MoveNext())
            {
                index++;
            }
            else
            {
                effects.RemoveAt(index);
            }
        }

        if (effects.Count == 0)
        {
            GlobalState.Instance.UpdateDispatcher.Updates.Remove(this);
        }
    }

    public void AddEffect(IEnumerator effect)
    {
        effects.Add(effect);

        if (effects.Count == 1)
        {
            GlobalState.Instance.UpdateDispatcher.Updates.Add(this);
        }
    }
}
