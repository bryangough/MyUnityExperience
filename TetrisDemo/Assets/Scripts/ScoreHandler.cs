/**
 * Author:    Bryan Gough
 * 
 *
 *
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {
	public Text scoreText;
	public GameHandler game;
	public void updateScore()
	{
		scoreText.text = game.score.ToString();
	}
}
