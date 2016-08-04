using UnityEngine;
using UnityEngine.SceneManagement;
using Service;

public class LevelTransition : MonoBehaviour
{
    protected void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<ReturnToMapEvent>(OnReturnToMap);
    }

    private void OnReturnToMap(ReturnToMapEvent gameEvent)
    {
        SceneManager.LoadScene(GlobalState.Instance.Services.Get<SceneService>().ReturnScene);
    }
}
