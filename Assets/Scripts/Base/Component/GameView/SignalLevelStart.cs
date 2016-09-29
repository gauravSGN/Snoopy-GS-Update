using UnityEngine;

public class SignalLevelStart : MonoBehaviour 
{
    protected void OnDestroy()
    {
        GlobalState.EventService.Dispatch(new LevelStartEvent());
    }
}
