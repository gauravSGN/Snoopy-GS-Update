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
        var sceneName = GlobalState.Instance.Services.Get<SceneService>().ReturnScene ?? mapSceneName;
        SceneManager.LoadScene(sceneName);
    }
}
