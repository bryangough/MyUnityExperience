using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public float progess;
	public Image bar;
	// Use this for initialization
	void Start () {
		progess = 0.0f;
		bar = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		bar.fillAmount = progess/100;
		if( progess>100 )
		{
			progess = 100;
		}
		if( progess<0 )
		{
			progess = 0;
		}
	}
}
