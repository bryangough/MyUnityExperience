using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	Dictionary<string, int> buttonStates = new Dictionary<string, int>();
	bool buttonStateChange = false;
	// Use this for initialization
	public void buttonUp (string buttonId) 
	{
		buttonStates[buttonId] = 0;
		buttonStateChange = true;
	}
	public void buttonDown(string buttonId)
	{
		buttonStates[buttonId] = 1;
		buttonStateChange = true;
	}

	public void buttonPress(string buttonId)
	{
		buttonStates[buttonId] = -1;
		buttonStateChange = true;
	}
	
	[Command]
	void CmdButtonChange(bool left, bool right, bool shoot)
	{
		Debug.Log("CmdButtonChange"+left+right+shoot);
	}
	// Update is called once per frame
	void Update () {
		if( buttonStateChange )
		{
			buttonStateChange = false;
			bool shoot = false;
			if(buttonStates["shoot"]==-1)
			{
				buttonStates["shoot"] = 0;
				shoot = true;
			}
			CmdButtonChange( (buttonStates["left"]==1), (buttonStates["left"]==1), shoot);
		}
	}
}
