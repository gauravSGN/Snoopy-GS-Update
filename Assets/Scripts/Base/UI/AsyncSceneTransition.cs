using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneTransition : MonoBehaviour
{
    public void TriggerSceneTransition()
    {
        GlobalState.SceneService.AllowTransition();
    }
}
