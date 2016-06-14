using Goal;

public class GoalCreatedEvent : BaseGoalEvent
{
    public GoalCreatedEvent(LevelGoal goal) : base(goal) { }
}
