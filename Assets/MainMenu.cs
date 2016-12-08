using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

	// Use this for initialization
	public void LoadScene (string sceneName)
	{
		SceneManager.LoadSceneAsync (sceneName);
	}

	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
