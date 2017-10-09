using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour {
	public Text playerLabel;
	public BoardDraughts theBoard;
	
	public WinScreen winScreen;
	public void playerChange()
	{
		if(theBoard.GetCurrentPlayer() == PieceColor.BLACK)
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
		winScreen.displayMe(theBoard.GetCurrentPlayer());
	}
	
	void Update () {
		
	}
}
