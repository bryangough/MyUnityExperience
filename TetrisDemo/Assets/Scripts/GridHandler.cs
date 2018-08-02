/**
 * Author:    Bryan Gough
 * 
 *
 *
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridHandler : MonoBehaviour {

	public int numberTilesW = 10;
    public int numberTilesH = 22;
	public float squareWidth;
	public float squareHeight;
	
	public Vector2 gameAreaOffset;
    public Transform[,] grid;

	RectTransform rectTransform;
	public Vector2 currentScreen;
	//
	public GameObject spawnPosition;
	void Start()
	{
		currentScreen.x = Screen.width;
		currentScreen.y = Screen.height;
		grid = new Transform[numberTilesW, numberTilesH];

		rectTransform = transform.GetComponent<RectTransform>();
		CalculateSizes();
	}

	void Update()
	{
		if(currentScreen.x != Screen.width || currentScreen.y != Screen.height)
		{
			CalculateSizes();
			RedoSize();
		}
	}
	public void CalculateSizes()
	{
		currentScreen.x = Screen.width;
		currentScreen.y = Screen.height;
		//A way to handle the gutters around the squares on the graphic.
		float tempGutterCalc = (float)4/240;
		float borederPercent = 1-2*tempGutterCalc;
		//

		//this math is my problem
		squareWidth = (rectTransform.rect.width*borederPercent)/numberTilesW;
		squareHeight = (rectTransform.rect.height*borederPercent)/(numberTilesH-2);
		//squareHeight = squareWidth;
		//
		gameAreaOffset.x = this.transform.position.x
				-(rectTransform.rect.width/2*borederPercent)
				+squareWidth/2
				+rectTransform.rect.width*tempGutterCalc;
		gameAreaOffset.y = rectTransform.rect.height*tempGutterCalc;

		spawnPosition.GetComponent<RectTransform>().localPosition = new Vector3(squareWidth/2, rectTransform.rect.height/2-squareHeight/2,0);
	}
	public Vector2 RoundAndApplyShift(Vector2 v) 
	{
    	return new Vector2(Mathf.Round((v.x-gameAreaOffset.x)/squareWidth),
    	                  Mathf.Round((v.y-gameAreaOffset.y)/squareHeight));
	}
	public bool IsInside(Vector2 pos) 
	{
		
    	return ( (int)pos.x >= 0 && (int)pos.x < numberTilesW && (int)pos.y >= 0 );
	}
	public void RemoveRow(int y) {
    	for (int x = 0; x < numberTilesW; ++x) {
        	Destroy(grid[x, y].gameObject);
        	grid[x, y] = null;
    	}
	}
	public void MoveRowsDown(int y) 
	{
		for (int x = 0; x < numberTilesW; ++x) 
		{
			if (grid[x, y] != null) 
			{
				// Move one towards bottom
				grid[x, y-1] = grid[x, y];
				grid[x, y] = null;

				// Update Block position
				grid[x, y-1].position += new Vector3(0, -squareHeight, 0);
        	}
    	}
	}
	public void MoveAnyAboveRowsDown(int y) 
	{
    	for (int i = y; i < numberTilesH; ++i)
		{
        	MoveRowsDown(i);
		}
	}
	public bool IsRowFull(int y) {
    	for (int x = 0; x < numberTilesW; ++x)
        	if (grid[x, y] == null)
            	return false;
    	return true;
	}


	public int DeleteFullRows() {
		int rowsRemoved = 0;
		for (int y = 0; y < numberTilesH; ++y) {
			if (IsRowFull(y)) {
				RemoveRow(y);
				MoveAnyAboveRowsDown(y+1);
				--y;
				rowsRemoved++;
			}
		}
		return rowsRemoved;
	}

	public void DeleteAllRows() {
		for (int y = 0; y < numberTilesH; ++y) {
			for (int x = 0; x < numberTilesW; ++x) {
				if(grid[x, y])
        			Destroy(grid[x, y].gameObject);
        		grid[x, y] = null;
    		}
		}
	}

	//Resizes all blocks on the grid.
	public void RedoSize()
	{
		for (int y = 0; y < numberTilesH; ++y) {
			for (int x = 0; x < numberTilesW; ++x) {
				if(grid[x, y])
        		{
					Vector3 pos = new Vector3(x * squareWidth + gameAreaOffset.x, y * squareHeight + gameAreaOffset.y, 0);
					grid[x, y].transform.position = pos;
					grid[x, y].GetComponent<RectTransform>().sizeDelta = new Vector2(squareWidth, squareHeight);
				}
    		}
		}
	}
}
