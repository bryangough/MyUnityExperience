﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ServerHandler : NetworkManager {

	public int numberOfPlayers = 0;
	public GameBoard gameBoard;
	public override void OnClientConnect(NetworkConnection conn) {
         ClientScene.AddPlayer(conn, 0);
     }
	/*public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		Debug.Log("first.");
	}*/

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
		Debug.Log("add player. ");
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Player playerComponent = player.GetComponent<Player>();
		//playerComponent.setup();
		playerComponent.gameBoard = gameBoard;
		if(numberOfPlayers==0)
		{
			playerComponent.setup(Team.blue);
		}
		else if(numberOfPlayers==1)
		{
			playerComponent.setup(Team.red);
		}
		else
		{
			Debug.Log("too many players!");
			
			return;
		}
		numberOfPlayers++;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		if(numberOfPlayers>=1)
		{
			gameBoard.startGame();
		}
    }

	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		numberOfPlayers--;
		//other player wins
		Debug.Log("disconnect player.");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
