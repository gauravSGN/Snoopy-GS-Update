namespace FSM
{
    public class ImmediateTransition : Transition
    {
        public override bool IsReady()
        {
            return true;
        }
    }
}