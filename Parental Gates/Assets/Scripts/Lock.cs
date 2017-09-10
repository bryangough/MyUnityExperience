using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {

	public ProgressBar bar;
	public bool progressFlag = false;
	public bool progressAllowed = true;
	public float progress = 0.0f;
	// Use this for initialization
	void Start () {
		bar = gameObject.GetComponentInChildren<ProgressBar>();
		progress = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(progress>100)
		{
			return;
		}
		if(progressFlag && progressAllowed)
		{
			progress += 70.0f * Time.deltaTime;
			
		}
		else
		{
			progress -= 300.0f * Time.deltaTime;
		}
		if(progress<0)
		{
			progress = 0;
		}
		if(bar)
		{
			bar.progess = progress;
		}
	}

	public void pointerDown()
	{
		progressFlag = true;
	}
	public void pointerUp()
	{
		progressFlag = false;
	}
}
