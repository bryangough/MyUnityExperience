﻿using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    protected int player;
    //next steps here

	public Board()
	{
		player = 1;
	}

	public virtual Move[] GetMoves()
	{
		return new Move[0];
	}

	public virtual Board MakeMove(Move m)
	{
		return new Board();
	}

	public virtual bool IsGameOver()
	{
		return true;
	}

	public virtual int GetCurrentPlayer()
	{
		return player;
	}

	

	public virtual float Evaluate(int player)
	{
		return Mathf.NegativeInfinity;
	}

	public virtual float Evaluate()
	{
		return Mathf.NegativeInfinity;
	}
}