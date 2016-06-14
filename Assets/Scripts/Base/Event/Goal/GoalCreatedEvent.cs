using Goal;

public class GoalCreatedEvent : GameEvent
{
    public LevelGoal Goal { get; private set; }

    public GoalCreatedEvent(LevelGoal goal)
    {
        Goal = goal;
    }
}
