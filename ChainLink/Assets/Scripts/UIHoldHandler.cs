using UnityEngine;
using System.Collections;

public class UIHoldHandler : MonoBehaviour 
{
	public PlayerSwinger player;
	public int chain = 0;
	void OnPress(bool isDown)
	{
		Debug.Log("on press " +isDown);
		player.climbFlag(chain, isDown);
	}
}
