using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public GameBoard gameBoard;
	
	[SyncVar]
	public Team myTeam;
	// Use this for initialization
	void Start () {
		GameObject g = GameObject.FindWithTag("Board");
		gameBoard = g.GetComponent<GameBoard>();
	}
	
	public void setup(Team team)
	{
		myTeam = team;
	}
	// Update is called once per frame
	void Update () 
	{
		if( isLocalPlayer && isYourTurn() )
		{
			Square square = null;
			//mobile
			if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
			{
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position), Vector2.zero);
				if(hit.collider != null)
				{
					square = hit.collider.gameObject.GetComponent<Square>();
				}
			}
			//editor
			if( Input.GetMouseButtonDown(0) )
			{
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null)
				{
					square = hit.collider.gameObject.GetComponent<Square>();
				}
			}
			if( square !=null )
			{
				if( square.team == Team.none )
				{
					CmdDoMove( square.x, square.y);
				}
			}
		}
	}
	/*void OnConnectedToServer() 
	{
        Debug.Log("Connected to server "+myTeam);
    }*/
	public override void OnStartLocalPlayer()
	{
		Debug.Log("Start me.");
	}
	public bool isYourTurn()
	{
		if(gameBoard==null)
			return false;
		return ( myTeam==gameBoard.turn );
	}
	[Command]
	public void CmdDoMove(int x, int y)
	{
		if (isServer)
		{
			gameBoard.addPiece(x,y, this.myTeam);
		}
	}

	/*[ClientRpc]
    void RpcYourTurn()
    {
		if( isLocalPlayer )
		{
			if(gameBoard.turn == myTeam)
			{
				Debug.Log(myTeam+"your turn!");
			}
		}
    }*/

	//is server ends or other player disconnects. Handle win.

}
