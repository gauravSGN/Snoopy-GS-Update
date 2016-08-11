using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelEditor
{
    public class ReturnToEditor : MonoBehaviour
    {
        public void Return()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }

        protected void OnLevelWasLoaded(int level)
        {
            if (SceneManager.GetActiveScene().name == LevelEditorConstants.SCENE_NAME)
            {
                Destroy(gameObject);
            }
        }
    }
}
