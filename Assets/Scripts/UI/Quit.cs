using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {

	public void QuitGame()
	{
		#if UNITY_EDITOR
		// set the PlayMode to stop
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif 
	}
}
