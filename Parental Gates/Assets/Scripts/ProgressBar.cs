using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Required for any new UI
using UnityEngine.UI;

//Helper class to drive the 
//This is kept seperate to easily change what the progress bar should look like.
public class ProgressBar : MonoBehaviour {

	float progress;
	Image bar;
	void Start () {
		progress = 0.0f;
		//This finds the first instance of Image on this gameObject
		//Grabbing it at the start makes things faster later instead of looking for it every time. 
		bar = gameObject.GetComponent<Image>();
	}
	
	void Update () {

	}
	//The is call by the external lock
	public void updateProgress(float progress)
	{
		this.progress = progress;
		//fillAmount is avaliable for Filled image type
		//Scale (bar.transform.localScale) could also be used to control the mask.
		bar.fillAmount = progress/100;
		//These are just some housekeeping to keep values tidy
		if( progress>100 )
		{
			progress = 100;
		}
		if( progress<0 )
		{
			progress = 0;
		}
	}
}
