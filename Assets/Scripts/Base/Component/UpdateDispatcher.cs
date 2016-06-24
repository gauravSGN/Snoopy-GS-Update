using System.Collections.Generic;
using UnityEngine;

public class UpdateDispatcher : MonoBehaviour
{
    public interface Receivers<in T>
    {
        void Add(T target);
        void Remove(T target);
    }

    private class ReceiverList<T> : Receivers<T>
    {
        public readonly List<T> receivers = new List<T>();

        public void Add(T target)
        {
            if (!receivers.Contains(target))
            {
                receivers.Add(target);
            }
        }

        public void Remove(T target)
        {
            receivers.Remove(target);
        }
    }

    private readonly ReceiverList<UpdateReceiver> updates = new ReceiverList<UpdateReceiver>();
    private readonly ReceiverList<LateUpdateReceiver> lateUpdates = new ReceiverList<LateUpdateReceiver>();
    private readonly ReceiverList<FixedUpdateReceiver> fixedUpdates = new ReceiverList<FixedUpdateReceiver>();

    public Receivers<UpdateReceiver> Updates { get { return updates; } }
    public Receivers<LateUpdateReceiver> LateUpdates { get { return lateUpdates; } }
    public Receivers<FixedUpdateReceiver> FixedUpdates { get { return fixedUpdates; } }

    public void Reset()
    {
        updates.receivers.Clear();
        lateUpdates.receivers.Clear();
        fixedUpdates.receivers.Clear();
    }

    protected void Update()
    {
        var receivers = updates.receivers;

        for (var index = receivers.Count - 1; index >= 0; index--)
        {
            receivers[index].OnUpdate();
        }
    }

    protected void LateUpdate()
    {
        var receivers = lateUpdates.receivers;

        for (var index = receivers.Count - 1; index >= 0; index--)
        {
            receivers[index].OnLateUpdate();
        }
    }

    protected void FixedUpdate()
    {
        var receivers = fixedUpdates.receivers;

        for (var index = receivers.Count - 1; index >= 0; index--)
        {
            receivers[index].OnFixedUpdate();
        }
    }
}
