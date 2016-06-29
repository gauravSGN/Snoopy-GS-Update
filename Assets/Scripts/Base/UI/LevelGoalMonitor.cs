using Goal;
using UnityEngine;
using Service;

public class LevelGoalMonitor : MonoBehaviour
{
    [SerializeField]
    private GoalType goalType;

    [SerializeField]
    TextUpdater updater;

    protected void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<GoalCreatedEvent>(GoalCreatedHandler);
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
