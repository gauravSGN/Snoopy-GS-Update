using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string mapSceneName;

    protected void Start()
    {
        EventDispatcher.Instance.AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
    }

    private void OnLevelComplete(LevelCompleteEvent gameEvent)
    {
        SceneManager.LoadScene(mapSceneName);
    }
}
