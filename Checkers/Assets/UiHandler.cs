using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour {
	public Text playerLabel;
	public BoardDraughts theBoard;
	
	public void playerChange()
	{
		playerLabel.text = "Player: "+theBoard.GetCurrentPlayer();
	}
	void Start () {
		playerChange();
		EventManager.StartListening ("PlayerChange", playerChange);
	}
	
	void Update () {
		
	}
}
