using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class JumpToScene : MonoBehaviour
{
    public KeyCode triggerKey;
    public string sceneName;
#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(triggerKey))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
#endif
}
