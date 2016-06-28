using Goal;
using UnityEngine;

public class LevelGoalMonitor : MonoBehaviour
{
    [SerializeField]
    private GoalType goalType;

    [SerializeField]
    TextUpdater updater;

    protected void Start()
    {
        GlobalState.Instance.EventDispatcher.AddEventHandler<GoalCreatedEvent>(GoalCreatedHandler);
        gameObject.SetActive(false);
    }

    private void GoalCreatedHandler(GoalCreatedEvent gameEvent)
    {
        if (gameEvent.Goal.Type == goalType)
        {
            updater.Target = gameEvent.Goal;
            gameObject.SetActive(true);
        }
    }
}
