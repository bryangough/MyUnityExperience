/**
 * Author:    Bryan Gough
 * 
 *
 *
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	public GameHandler game;
	
	public KeyCode leftKey =  KeyCode.LeftArrow;
	public KeyCode rightKey = KeyCode.RightArrow;
	public KeyCode downKey = KeyCode.DownArrow;
	public KeyCode rotateClockwise = KeyCode.F;
	public KeyCode rotateCounterClockwise = KeyCode.G;

	public bool downKeyPressed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!game.currentBlock)
		{
			return;
		}
		if (Input.GetKeyDown(leftKey))
		{
			game.currentBlock.DoMove(new Vector3(-1,0,0));
		}
		if (Input.GetKeyDown(rightKey))
		{
			game.currentBlock.DoMove(new Vector3(1,0,0));
		}
		if (Input.GetKeyDown(downKey))
        {
			downKeyPressed = true;
		}
		if (Input.GetKeyUp(downKey))
		{
			downKeyPressed = false;
		}
		if(downKeyPressed)
		{
			game.currentBlock.DoMoveDown();
		}
		//rotate
		if (Input.GetKeyDown(rotateClockwise))
        {
			game.currentBlock.DoRotate(90);
		}
		
		if (Input.GetKeyDown(rotateCounterClockwise))
        {
			game.currentBlock.DoRotate(-90);
		}
	}
}
