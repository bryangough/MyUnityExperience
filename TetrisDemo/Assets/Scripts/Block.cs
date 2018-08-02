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
public class Block : MonoBehaviour {


	public GridHandler grid;
	public GameHandler game;
	public GameObject innerBlock;
	public Sprite blockSprite;
	
	public bool[] objectGrid = new bool[16];
	void Start() 
	{
		for(int x=0;x<objectGrid.Length;x++)
		{
			int posx = (int)x/4;
			int posy = (int)x%4;
			
			if(objectGrid[x])
			{
				//Vector3 pos = new Vector3(posx*grid.squareWidth-2*grid.squareWidth, posy*grid.squareHeight-grid.squareHeight,0);
				Vector3 pos = new Vector3(posx*grid.squareWidth-grid.squareWidth, posy*grid.squareHeight-grid.squareHeight,0);
				GameObject obj = Instantiate(innerBlock, Vector3.zero, Quaternion.identity);
				//
				obj.transform.SetParent(transform);
				obj.transform.localPosition = pos;
				//
				Image image = obj.GetComponent<Image>();
				image.sprite =  blockSprite;
				obj.GetComponent<RectTransform>().sizeDelta = new Vector2(grid.squareWidth, grid.squareHeight);				
			}
		}

		if (!IsValidBlockPosition())
		{
			game.GameOver();
		}
	}

	
	
	// Update is called once per frame
	void Update () {
		
	}
	public bool DoMove(Vector3 move)
	{
		transform.position += move * grid.squareWidth;
        
        // See if valid
        if (IsValidBlockPosition())
		{
			UpdateGrid();
			return true;
		}
        else
		{
			transform.position += move * grid.squareWidth * -1;
			return false;
		}  
	}
	public bool DoRotate(int direction)
	{
		transform.Rotate(0, 0, direction);
		if ( IsValidBlockPosition() )
		{
			UpdateGrid();
			//rotate inners so shadows are correct
			foreach (Transform child in transform) 
			{
				child.Rotate(0,0, -1 * direction);
			}
			return true;
		}
        else
		{
			//TODO** I should shift the blocks to try to find a successful rotate

			//
			transform.Rotate(0, 0, direction*-1);
			return false;
		} 
	}
	public bool DoMoveDown()
	{		
		transform.position += new Vector3(0, - grid.squareHeight, 0);
		if (IsValidBlockPosition()) 
		{
			UpdateGrid();
			return true;
		}
		else
		{
			transform.position += new Vector3(0, grid.squareHeight, 0);
			game.gainPoints( grid.DeleteFullRows() );
			game.SpawnNext();
			enabled = false;
			return false;
		}
	}

	//move these to grid
	bool IsValidBlockPosition() {        
		foreach (Transform child in transform) {
			Vector2 v = grid.RoundAndApplyShift(child.position);

			if (!grid.IsInside(v))
				return false;
//			Debug.Log("isValidGridPos "+this.name+" "+child.name+v);
			if (grid.grid[(int)v.x, (int)v.y] != null &&
				grid.grid[(int)v.x, (int)v.y].parent != transform)
				return false;
		}
		return true;
	}

	void UpdateGrid() {
		for (int y = 0; y < grid.numberTilesH; ++y)
			for (int x = 0; x < grid.numberTilesW; ++x)
				if (grid.grid[x, y] != null)
					if (grid.grid[x, y].parent == transform)
						grid.grid[x, y] = null;

		foreach (Transform child in transform) {
			Vector2 v = grid.RoundAndApplyShift(child.position);
			grid.grid[(int)v.x, (int)v.y] = child;
		}        
	}
}
