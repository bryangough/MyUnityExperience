using UnityEngine;
using System.Collections;

public class Board
{
    protected PieceColor player;
    //next steps here

	public Board()
	{
		player = PieceColor.BLACK;
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

	public virtual PieceColor getCurrentPlayer()
	{
		return player;
	}

	

	public virtual float Evaluate(PieceColor player)
	{
		return Mathf.NegativeInfinity;
	}

	public virtual float Evaluate()
	{
		return Mathf.NegativeInfinity;
	}
}