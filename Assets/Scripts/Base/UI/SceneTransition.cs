using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

	public void TriggerSceneTransition(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

}
