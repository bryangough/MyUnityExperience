using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The lock handler.
//More locks can be added by 
public class MultiLockHandler : MonoBehaviour {

	//The list of locks to be used for this gate
	public Lock[] locks;
	//How many locks are currently pressed
	public int locksPress = 0;
	//The number of locks needed to open the gate.
	public int neededLocks = 3;
	//The panel this should switch to when the locks are opened
	public GameObject parentPanel;
	//Iterator for the for loops. I should find out if this is still best practice.
	int iterator;
	//For demonstration purposes I am showing this screen on 
	//For use within a larger app, showParentPermissions should be commented out and 
	//closePermissions called instead. Another object should then call showParentPermissions when
	//You want to display the parent section.
	void Start () {
		showParentPermissions();
		//closePermissions();
	}
	
	// Update is called once per frame
	//
	void Update () {
		int finished = 0;
		for(iterator=0;iterator<locks.Length;iterator++)
		{
			if( locks[iterator].progress>=100.0f )
			{
				finished++;
			}
		}
		if( finished>=neededLocks )
		{
			permissionsPassed();
		}
	}
	//This resets all lockers progress to 0 and disable their current press. 
	void reset()
	{
		for(iterator=0;iterator<locks.Length;iterator++)
		{
			locks[iterator].progress = 0;
			locks[iterator].progressAllowed = false;
		}
	}
	//These functions handle showing the two screens: the Parent Gate and the actual Parent Panel
	public void showParentPermissions()
	{
		reset();
		gameObject.SetActive(true);
		parentPanel.SetActive(false);
	}
	public void closePermissions()
	{
		gameObject.SetActive(false);
		parentPanel.SetActive(false);
	}
	public void permissionsPassed()
	{
		gameObject.SetActive(false);
		parentPanel.SetActive(true);
	}
	// These are called by the button's Event Trigger script
	// Both help keep a count of how many buttons are currently pressed
	public void lockPress()
	{
		locksPress++;
		Debug.Log("press"+locksPress);
		if(locksPress >= neededLocks)
		{
			for(iterator=0;iterator<locks.Length;iterator++)
			{
				locks[iterator].progressAllowed = true;
			}
		}
	}
	
	public void lockReleased()
	{
		locksPress--;
		if( locksPress<0 )
		{
			Debug.Log("Error: Move locks released?");
			locksPress = 0;
		}
		Debug.Log("release"+locksPress);
		if(locksPress < neededLocks)
		{
			
			for(iterator=0;iterator<locks.Length;iterator++)
			{
				locks[iterator].progressAllowed = false;
			}
		}
	}
}
