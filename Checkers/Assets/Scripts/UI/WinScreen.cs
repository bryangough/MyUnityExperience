using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WinScreen : MonoBehaviour {

	public Text winText;
	
	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void displayMe(PieceColor player)
	{
		this.gameObject.SetActive(true);
		winText.text = "Winner is "+player;
	}


	public void resetGame()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	public void returnToLobby()
	{

	}
}
