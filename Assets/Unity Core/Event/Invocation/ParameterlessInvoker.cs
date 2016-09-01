namespace Event.Invocation
{
    sealed public class ParameterlessInvoker : Invoker
    {
        public object Target { get { return Handler; } }
        public System.Action Handler { get; set; }

        public void Invoke(GameEvent gameEvent)
        {
            Handler.Invoke();
        }
    }
}
