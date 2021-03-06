﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour {
	public Text playerLabel;
	public GameBoard theBoard;
	
	public WinScreen winScreen;
	public void playerChange()
	{
		if(theBoard.board.getCurrentPlayer() == PieceColor.BLACK)
		{
			playerLabel.text = "Player: RED";
		}
		else
		{
			playerLabel.text = "Player: BLUE";
		}
	}
	void Start () {
		playerChange();
		EventManager.StartListening ("PlayerChange", playerChange);
		EventManager.StartListening ("WinGame", showWinScreen);
	}

	public void showWinScreen()
	{
		//theBoard
		winScreen.displayMe(theBoard.board.getCurrentPlayer());
	}
	
	void Update () {
		
	}
}
