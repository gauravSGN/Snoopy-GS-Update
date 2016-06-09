using UnityEngine;

public class DontDeleteOnLoad : MonoBehaviour
{
	void Awake() 
	{
		DontDestroyOnLoad(gameObject);
	}
}
