using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LoopHandler : MonoBehaviour 
{
	public GameObject winScreen;
	public GameObject pauseScreen;  
	void Start()
	{
		pauseScreen.SetActive(false);
		winScreen.SetActive(false);
	}
	public void showOptions()
	{
		pauseScreen.SetActive(true);
		HexFieldBasicWithWater.pause = true;
	}
	public void closeOption()
	{
		pauseScreen.SetActive(false);
		HexFieldBasicWithWater.pause = false;
	}
	public void showWinScreen()
	{
		winScreen.SetActive(true);
		HexFieldBasicWithWater.pause = true;
	}

	//
	public void resetGame()
	{
		Application.LoadLevel("LoopGame");
	}
	public void menu()
	{
		Application.LoadLevel("MainMenu");
	}
}
