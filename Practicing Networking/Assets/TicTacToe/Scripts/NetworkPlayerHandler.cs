using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerHandler : NetworkBehaviour {

	public GameBoard gameBoard;
	
	[SyncVar]
	public Team myTeam;

	public void touchSpot(Square square)
	{
		if( isLocalPlayer )
		{
			CmdDoMove( square.x, square.y);
		}
	}
	// Use this for initialization
	[Command]
	public void CmdDoMove(int x, int y)
	{
		if (isServer)
		{
			gameBoard.addPiece(x,y, this.myTeam);
		}
	}

	public void setup(Team team)
	{
		myTeam = team;
	}
}
