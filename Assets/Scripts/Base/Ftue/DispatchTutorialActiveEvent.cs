using UnityEngine;
using FTUE;

public class DispatchTutorialActiveEvent : MonoBehaviour 
{
    protected void OnEnable()
    {
        GlobalState.EventService.Dispatch<TutorialActiveEvent>(new TutorialActiveEvent(true));
    }

    protected void OnDestroy()
    {
        GlobalState.EventService.Dispatch<TutorialActiveEvent>(new TutorialActiveEvent(false));
    }
}
