using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelEditor
{
    public class ReturnToEditor : MonoBehaviour
    {
        public void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Return()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.GetActiveScene().name == LevelEditorConstants.SCENE_NAME)
            {
                Destroy(gameObject);
            }
        }
    }
}
