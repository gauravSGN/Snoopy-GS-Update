using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string mapSceneName;

    protected void Start()
    {
        GlobalState.Instance.EventDispatcher.AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
    }

    private void OnLevelComplete(LevelCompleteEvent gameEvent)
    {
        var sceneName = GlobalState.Instance.returnScene ?? mapSceneName;
        SceneManager.LoadScene(sceneName);
    }
}
