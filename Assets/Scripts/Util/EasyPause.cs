using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class EasyPause : MonoBehaviour {
    public KeyCode pauseKey;
#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            EditorApplication.isPaused = true;
        }
    }
#endif
}
