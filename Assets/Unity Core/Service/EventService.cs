using Event;
using System;

namespace Service
{
    public interface EventService : SharedService
    {
        EventRegistry Transient { get; }
        EventRegistry Persistent { get; }

        void AddEventHandler<T>(Action<T> handler) where T : GameEvent;
        void RemoveEventHandler<T>(Action<T> handler) where T : GameEvent;

        void Dispatch<T>(T gameEvent) where T : GameEvent;
        void DispatchPooled<T>(T gameEvent) where T : GameEvent;

        T GetPooledEvent<T>() where T : GameEvent;
        void AddPooledEvent<T>(T gameEvent) where T : GameEvent;
    }
}
