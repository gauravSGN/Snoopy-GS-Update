namespace FSM
{
    public class MessageReceivedTransition : Transition
    {
        public string messageName;

        private bool ready;

        public override void OnEnter()
        {
            ready = false;
        }

        public override bool IsReady()
        {
            return ready;
        }

        public override void HandleMessage(string message)
        {
            if(message == messageName)
            {
                ready = true;
            }
        }
    }
}