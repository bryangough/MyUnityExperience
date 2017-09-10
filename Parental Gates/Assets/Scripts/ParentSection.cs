using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is required for using SceneManager
using UnityEngine.SceneManagement;
//This is a basic framework to start the Parent Section
//A reset scene button was placed here for convenience.

public class ParentSection : MonoBehaviour {

	public void showPanel()
	{
		gameObject.SetActive(true);
	}
	public void closePanel()
	{
		gameObject.SetActive(false);
	}

	//This is a testing function so you aren't stuck on the Parent Section after
	//SceneManager.LoadScene accept either a scene name or number (based on build settings)
	//Loading the same scene will reset the current.
	public void resetScene()
	{
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}
}
