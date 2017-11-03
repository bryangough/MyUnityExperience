using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//the graphic representation of the game
//the view
public class GameBoard : NetworkBehaviour {

	public Camera boardCamera;
	public Color blueColour;
	public Color redColour;
	public GameObject redObject;
	public GameObject blueObject;

	public Square[] tiles;
	
	public NetworkGameBoard boardModel;
	// Use this for initialization
	void Start()
    {

    }
	
	public bool calculateIfWin(Team turn)
	{
		int[,] winSpots = new int[,] { 
				{ 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
				{ 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
				{ 0, 4, 5 }, { 2, 4, 6 }
			};
		//this is super lazy
		int count = 0;
		int length = winSpots.GetLength(0);
		for(var x = 0; x < length; x++)
		{
			count = 0;
			for( var y = 0; y < 3; y++)	
			{
				if( tiles[ winSpots[x,y] ].location.team != turn )
				{
					break;
				}
				else
				{
					count++;
				}
			}
			if(count == 3)
			{
				return true;
			}
		}
		return false;
	}
	public Square getSquare(int x, int y)
	{
		if(x>=0 && x<3 && y>=0 && y<3)
		{
			return tiles[x+y*3];
		}
		return null;
	}
	public void changeTurn()
	{
		if (!isServer)
            return;

		if(turn == Team.blue)
		{
			turn = Team.red;
		}
		else
		{
			turn = Team.blue;
		}
		//
		RpcChangeTurn(turn);
	}
	
	void boardChanged(SyncListStruct<SquareModel>.Operation op, int itemIndex)
    {
		if(op == SyncListStruct<SquareModel>.Operation.OP_SET)
		{
			SquareModel model = theBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			addMarker(s, model.team);
		}
		if(op == SyncListStruct<SquareModel>.Operation.OP_ADD)
		{
			SquareModel model = theBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			s.location = model;
		}
    }
	public void addMarker(Square square, Team team)
	{
		if( team == Team.none)
			return;
		GameObject prefab = redObject;
		if( team == Team.blue)
		{
			prefab = blueObject;
		}
		GameObject player = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
		player.transform.position = square.transform.position;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
