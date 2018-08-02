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
using UnityEngine.Events;
public class GameHandler : MonoBehaviour {

	public Block[] possibleBlocks;
	public Block currentBlock;
	
	public GridHandler grid; 
	//How quickly the game will update
	public float gameUpdateStepTime = 1;
	[HideInInspector]
	public float lastDrop = 0;
	public int score = 0;
	public bool gameOver = false;
	public GameObject resetPanel;

	public UnityEvent updateScore;
	public int removedRowAward = 100;

	public void SpawnNext() 
	{
		if(gameOver)
			return;
		lastDrop = 0;
    	int i = Random.Range(0, possibleBlocks.Length);
    	Block spawned = Instantiate(possibleBlocks[i], grid.spawnPosition.transform.position, Quaternion.identity);
		spawned.transform.SetParent(transform);
		spawned.grid = grid;
		spawned.game = this;
		currentBlock = spawned;
	}
	//ui
	// Use this for initialization
	void Start () 
	{
		
		SpawnNext();
	}
	
	void Update () 
	{
		if( !gameOver && currentBlock ) 
		{
			lastDrop += Time.deltaTime;
			if( lastDrop>gameUpdateStepTime )
			{
				currentBlock.DoMoveDown();
				lastDrop = 0;
			}
		}
	}

	public void gainPoints(int newPoints)
	{
		score += newPoints * removedRowAward;
		updateScore.Invoke();
	}
	public void GameOver()
	{
		gameOver = true;
		//Debug.Log("gameover");
		//display reset
		resetPanel.SetActive(true);
	}

	public void RestartGame()
	{
		if(currentBlock)
			Destroy(currentBlock.gameObject);
		currentBlock = null;
		//remove all blocks
		grid.DeleteAllRows();
		//
		score = 0;
		updateScore.Invoke();
		gameOver = false;
		SpawnNext();
		resetPanel.SetActive(false);
	}
	

}
