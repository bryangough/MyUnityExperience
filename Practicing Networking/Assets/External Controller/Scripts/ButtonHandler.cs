using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {
	public string id;
	public PlayerController controller;
	public void buttonUp () {
		if( controller != null )
		{
			controller.buttonUp(id);
		}
	}
	public void buttonDown()
	{
		if( controller != null )
		{
			controller.buttonDown(id);
		}
	}

	public void buttonPress()
	{
		if( controller != null )
		{
			controller.buttonPress(id);
		}
	}
}
