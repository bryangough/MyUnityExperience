using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {

	//
	ProgressBar bar;
	//
	bool progressFlag = false;
	//
	public bool progressAllowed = true;
	//
	public float progress = 0.0f;

	//The speed the locks progress will change when pressed
	public float increaseSpeed = 70.0f;
	//The speed the locks progress will change when the button is released
	public float decreaseSpeed = 300.0f;
	// Use this for initialization
	void Start () {
		//GetComponentInChildren gets the first ProgressBar in any of the gameObjects below this one
		bar = gameObject.GetComponentInChildren<ProgressBar>();
		//reseting Progress at the start. Not the f is required for floats.
		progress = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(progress>100)
		{
			return;
		}
		//
		if(progressFlag && progressAllowed)
		{
			progress += increaseSpeed * Time.deltaTime;
			
		}
		else
		{
			progress -= decreaseSpeed * Time.deltaTime;
		}
		if(progress<0)
		{
			progress = 0;
		}
		//This is a null check. If bar doesn't exist, we don't try to pass it anything
		if(bar)
		{
			bar.updateProgress( progress );
		}
	}
	
	// These are called by the button's Event Trigger script
	//This is a flag system. The update call is where the work is done.
	public void pointerDown()
	{
		progressFlag = true;
	}
	public void pointerUp()
	{
		progressFlag = false;
	}
}
