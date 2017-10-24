using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkGameBoard : NetworkBehaviour {

	public Camera boardCamera;
	public Color blueColour;
	public Color redColour;

	public Square[] tiles;

	public GameObject redObject;
	public GameObject blueObject;

	[SyncVar]
	public Team turn = Team.none;
	

	public class TheBoard : SyncListStruct<SquareModel> 
	{

	}
    TheBoard theBoard = new TheBoard();
	
	// Use this for initialization
	void Start()
    {
        theBoard.Callback = boardChanged;
		
		if(!isServer)
			return;
		SquareModel squareModel;
		for(var x=0;x<tiles.Length;x++)
		{
			squareModel = new SquareModel();
			squareModel.x = tiles[x].x;
			squareModel.y = tiles[x].y;
			tiles[x].location = squareModel;
			theBoard.Add(squareModel);
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
		Square s = getSquare(x,y);
		if( s!=null )
		{
			//s.team = team;
			
			int index = theBoard.IndexOf(s.location);
			SquareModel m = s.location;
			SquareModel newM = new SquareModel(m.x, m.y, team);
			s.location = newM;
			//m.team = team;
			if(index!=-1)
			{
				theBoard[index] = newM;
			}
			if( calculateIfWin() )
			{
		//wins
		//[1,2,3],[4,5,6],[7,8,9]
		//[1,4,7],[2,5,8],[3,6,9]
		//[1,5,6],[3,5,7]

		//display winscreen
		//
			}
			else
			{
				changeTurn();
			}
		}
	}
	public bool calculateIfWin()
	{
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
			SquareModel model = theBoard.GetItem(itemIndex);
			Square s = getSquare(model.x, model.y);
			addMarker(s, model.team);
		}
		if(op == SyncListStruct<SquareModel>.Operation.OP_ADD)
		{
			SquareModel model = theBoard.GetItem(itemIndex);
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
