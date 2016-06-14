namespace FSM
{
    public class WaitForDoneSignalTransition : Transition
    {
        private bool done = false;

        public void OnEnter()
        {
            done = false;
        }

        public override bool IsReady()
        {
            return done;
        }

        public void SetDone()
        {
            done = true;
        }
    }
}
