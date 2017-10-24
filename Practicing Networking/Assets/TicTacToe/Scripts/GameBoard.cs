﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameBoard : NetworkBehaviour {

	public Camera boardCamera;
	public Color blueColour;
	public Color redColour;

	public Square[] tiles;

	public GameObject redObject;
	public GameObject blueObject;

	[SyncVar]
	public Team turn = Team.none;
	
	[SyncVar]
	public bool gameEnded = false;
	

	public class TheBoard : SyncListStruct<BoardModel> 
	{

	}
    TheBoard theBoard = new TheBoard();
	
	// Use this for initialization
	void Start()
    {
        theBoard.Callback = boardChanged;
		
		if(!isServer)
			return;
		BoardModel boardModel;
		for(var x=0;x<tiles.Length;x++)
		{
			boardModel = new BoardModel();
			boardModel.x = tiles[x].x;
			boardModel.y = tiles[x].y;
			tiles[x].location = boardModel;
			theBoard.Add(boardModel);
		}
		//theBoard.Add()
    }
	
	public void startGame()
	{
		//if(Random.value<0.5)
		//{
			turn = Team.blue;
		/*}
		else
		{
			turn = Team.red;
		}*/
		RpcChangeTurn(turn);
	}
	public void addPiece(int x, int y, Team team)
	{
		if(gameEnded)
			return;

		Square s = getSquare(x,y);
		if( s!=null )
		{
			//s.team = team;
			
			int index = theBoard.IndexOf(s.location);
			BoardModel m = s.location;
			BoardModel newM = new BoardModel(m.x, m.y, team);
			s.location = newM;
			//m.team = team;
			if(index!=-1)
			{
				theBoard[index] = newM;
			}
			if( calculateIfWin(team) )
			{
				gameEnded = true;
				RpcEndGame(team);
				//end game
			}
			else
			{
				changeTurn();
			}
		}
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
		for(var x = 0; x < winSpots.Length; x++)
		{
			count = 0;
			for( var y = 0; y < 3; y++)	
			{

				if( tiles[x].location.team != turn )
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
	[ClientRpc]
	void RpcEndGame(Team winner)
	{
		//display winscreen
		Debug.Log("winner "+winner);
		if(winner == Team.blue)
		{
			boardCamera.backgroundColor = blueColour;
		}
		else
		{
			boardCamera.backgroundColor = redColour;
		}
	}
	[ClientRpc]
    void RpcChangeTurn(Team newTurn)
    {
		Debug.Log("changeturns "+isLocalPlayer+" "+newTurn);
		if(newTurn == Team.blue)
		{
			boardCamera.backgroundColor = blueColour;
		}
		else
		{
			boardCamera.backgroundColor = redColour;
		}
    }
	void boardChanged(SyncListStruct<BoardModel>.Operation op, int itemIndex)
    {
		if(op == SyncListStruct<BoardModel>.Operation.OP_SET)
		{
			BoardModel model = theBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			addMarker(s, model.team);
		}
		if(op == SyncListStruct<BoardModel>.Operation.OP_ADD)
		{
			BoardModel model = theBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			//Debug.Log("add s"+s);
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
