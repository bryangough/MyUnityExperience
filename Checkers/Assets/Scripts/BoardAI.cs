using UnityEngine;
using System.Collections;

public class BoardAI
{
	public static float Minimax(
			Board board,
			PieceColor player,
			int maxDepth,
			int currentDepth,
			ref Move bestMove)
	{
		if (board.IsGameOver() || currentDepth == maxDepth)
    		return board.Evaluate(player);

		bestMove = null;
		float bestScore = Mathf.Infinity;
		if (board.GetCurrentPlayer() == player)
    		bestScore = Mathf.NegativeInfinity;

		foreach (Move m in board.GetMoves())
		{
			Board b = board.MakeMove(m);
			float currentScore;
			Move currentMove = null;
			currentScore = Minimax(b, player, maxDepth, currentDepth + 1, ref currentMove);
			if (board.GetCurrentPlayer() == player)
			{
				if (currentScore > bestScore)
				{
					bestScore = currentScore;
					bestMove = currentMove;
				}
			}
			else
			{
				if (currentScore < bestScore)
				{
					bestScore = currentScore;
					bestMove = currentMove;
				}
			}
		}
		return bestScore;
	}
}