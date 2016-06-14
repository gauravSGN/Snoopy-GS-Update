namespace Goal
{
    public abstract class LevelGoal : Observable
    {
        public abstract GoalType Type { get; }
        public int CurrentValue { get; protected set; }
        public int TargetValue { get; protected set; }
        public bool Complete { get; private set; }

        public abstract void Initialize(LevelData levelData);

        protected void CompleteGoal()
        {
            Complete = true;
            EventDispatcher.Instance.Dispatch(new GoalCompleteEvent(this));
        }
    }
}
