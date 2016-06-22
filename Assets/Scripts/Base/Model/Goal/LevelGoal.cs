namespace Goal
{
    abstract public class LevelGoal : Observable
    {
        abstract public GoalType Type { get; }
        public int CurrentValue { get; protected set; }
        public int TargetValue { get; protected set; }
        public bool Complete { get; private set; }

        abstract public void Initialize(LevelData levelData);

        protected void CompleteGoal()
        {
            Complete = true;
            GlobalState.Instance.EventDispatcher.Dispatch(new GoalCompleteEvent(this));
        }
    }
}
