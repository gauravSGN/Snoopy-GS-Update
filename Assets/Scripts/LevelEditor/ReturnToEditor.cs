using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Service;

namespace LevelEditor
{
    public class ReturnToEditor : MonoBehaviour
    {
        public void Return()
        {
            SceneManager.LoadScene(GlobalState.Instance.Services.Get<SceneService>().ReturnScene);
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
