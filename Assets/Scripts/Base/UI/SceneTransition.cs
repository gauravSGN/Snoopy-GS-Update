using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private TextAsset nextLevelData;

    [SerializeField]
    private int levelNumber;

    public void TriggerSceneTransition(string sceneName)
    {
        if (GlobalState.Instance.Services.Get<UserStateService>().purchasables.hearts.quantity > 0)
        {
            var sceneData = GlobalState.Instance.Services.Get<SceneService>();

            if (nextLevelData != null)
            {
                sceneData.NextLevelData = nextLevelData.text;
            }

            sceneData.LevelNumber = levelNumber;
            sceneData.ReturnScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(sceneName);
        }
    }
}
