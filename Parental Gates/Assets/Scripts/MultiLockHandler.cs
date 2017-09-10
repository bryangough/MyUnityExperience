using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLockHandler : MonoBehaviour {

	public Lock[] locks;
	// Use this for initialization
	public int locksPress = 0;
	public int neededLocks = 3;
	public GameObject parentPanel;
	private int iterator;
	void Start () {
		showParentPermissions();
	}
	void reset()
	{
		gameObject.SetActive(true);
		for(iterator=0;iterator<locks.Length;iterator++)
		{
			locks[iterator].progress = 0;
			locks[iterator].progressAllowed = false;
		}
	}
	// Update is called once per frame
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
