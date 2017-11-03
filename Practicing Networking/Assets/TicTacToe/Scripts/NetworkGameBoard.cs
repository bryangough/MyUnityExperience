using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//network represntation of the game
public class NetworkGameBoard : NetworkBehaviour {

	

	[SyncVar]
	public Team turn = Team.none;
	
	[SyncVar]
	public bool gameEnded = false;
	
	public GameBoard boardGraphics;

	public class TheBoard : SyncListStruct<SquareModel> 
	{

	}
    TheBoard syncedBoard = new TheBoard();
	public void createBoard()
	{
		SquareModel boardModel;
		Square[] tiles = boardGraphics.tiles;
		for(var x=0;x<tiles.Length;x++)
		{
			boardModel = new SquareModel();
			boardModel.x = tiles[x].x;
			boardModel.y = tiles[x].y;
			tiles[x].location = boardModel;
			syncedBoard.Add(boardModel);
		}
	}
	
	// Use this for initialization
	void Start()
    {

        syncedBoard.Callback = boardChanged;
		
		if(!isServer)
			return;
		
		createBoard();
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
			
			int index = syncedBoard.IndexOf(s.location);
			SquareModel m = s.location;
			SquareModel newM = new SquareModel(m.x, m.y, team);
			s.location = newM;
			//m.team = team;
			if(index!=-1)
			{
				syncedBoard[index] = newM;
			}
			if( calculateIfWin(team) )
			{
				gameEnded = true;
				RpcEndGame(team);
			}
			else
			{
				changeTurn();
			}
		}
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
	void boardChanged(SyncListStruct<SquareModel>.Operation op, int itemIndex)
    {
		if(op == SyncListStruct<SquareModel>.Operation.OP_SET)
		{
			SquareModel model = syncedBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			addMarker(s, model.team);
		}
		if(op == SyncListStruct<SquareModel>.Operation.OP_ADD)
		{
			SquareModel model = syncedBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			//Debug.Log("add s"+s);
			s.location = model;
		}
    }
}
