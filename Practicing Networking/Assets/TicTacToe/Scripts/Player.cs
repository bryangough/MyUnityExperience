using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

	public GameBoard gameBoard;
	public Team myTeam;

	public NetworkPlayerHandler networkPlayer;
	// Use this for initialization
	void Start () {
		GameObject g = GameObject.FindWithTag("Board");
		gameBoard = g.GetComponent<GameBoard>();

		networkPlayer = gameObject.GetComponent<NetworkPlayerHandler>();

		if( networkPlayer )
		{
			this.gameBoard = networkPlayer.gameBoard;
			this.myTeam = networkPlayer.myTeam;
		}
	}
	
	public void setup(Team team)
	{
		myTeam = team;
	}
	// Update is called once per frame
	void Update () 
	{
		//isLocalPlayer && 
		if( isYourTurn() )
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
					if(networkPlayer != null)
					{
						networkPlayer.touchSpot(square);
					}
					else
					{
						gameBoard.addPiece(square.x, square.y, this.myTeam);
					}
				}
			}
		}
	}
	public bool isYourTurn()
	{
		if(gameBoard==null)
			return false;
		return ( myTeam==gameBoard.turn );
	}
}
