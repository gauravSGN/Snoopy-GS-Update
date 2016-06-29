using UnityEngine;
using UnityEngine.SceneManagement;
using Service;

public class LevelTransition : MonoBehaviour
{
    public string mapSceneName;

    protected void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
    }

    private void OnLevelComplete(LevelCompleteEvent gameEvent)
    {
        var sceneName = GlobalState.Instance.returnScene ?? mapSceneName;
        SceneManager.LoadScene(sceneName);
    }
}
