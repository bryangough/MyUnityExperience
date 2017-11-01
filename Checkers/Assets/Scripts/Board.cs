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

	public virtual Move[] getMoves()
	{
		return new Move[0];
	}

	public virtual BoardModel makeMove(Move m)
	{
		return new BoardModel();
	}

	public virtual bool isGameOver()
	{
		return true;
	}

	public virtual PieceColor getCurrentPlayer()
	{
		return player;
	}
}