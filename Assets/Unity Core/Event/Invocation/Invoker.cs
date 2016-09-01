namespace Event.Invocation
{
    public interface Invoker
    {
        object Target { get; }

        void Invoke(GameEvent gameEvent);
    }
}
