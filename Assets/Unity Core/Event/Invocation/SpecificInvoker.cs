namespace Event.Invocation
{
    sealed public class SpecificInvoker<T> : Invoker where T : GameEvent
    {
        public object Target { get { return Handler; } }
        public System.Action<T> Handler { get; set; }

        public void Invoke(GameEvent gameEvent)
        {
            Handler.Invoke((T)gameEvent);
        }
    }
}
