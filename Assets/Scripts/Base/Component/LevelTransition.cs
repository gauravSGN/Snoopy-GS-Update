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
        SceneManager.LoadScene(mapSceneName);
    }
}
