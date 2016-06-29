using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneTransition : MonoBehaviour
{
    public void TriggerSceneTransition(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        GlobalState.Instance.Services.Get<TopUIService>().ShowLoading(op);
    }
}
