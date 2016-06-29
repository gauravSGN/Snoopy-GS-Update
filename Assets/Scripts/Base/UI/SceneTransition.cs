using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private TextAsset nextLevelData;

    public void TriggerSceneTransition(string sceneName)
    {
        if (nextLevelData != null)
        {
            GlobalState.Instance.Services.Get<SceneService>().NextLevelData = nextLevelData.text;
        }

        SceneManager.LoadScene(sceneName);
    }
}
