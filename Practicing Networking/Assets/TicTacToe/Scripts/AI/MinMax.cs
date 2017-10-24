using UnityEngine;
using System.Collections;

public class BoardAI
{
	public static float Minimax(
			BoardModel board,
			Team player,
			int maxDepth,
			int currentDepth,
			AIModel aI,
			ref SquareModel bestMove)
	{
		if (board.isWin() || currentDepth == maxDepth)
    		return aI.evaluate(board, player);

		//bestMove = null;
		float bestScore = Mathf.Infinity;
		if (board.currentTeam() == player)
    		bestScore = Mathf.NegativeInfinity;

		foreach (SquareModel m in board.getMoves())
		{
			BoardModel b = board.doMoveClone(m);
			float currentScore;
			SquareModel currentMove = m;
			currentScore = Minimax(b, player, maxDepth, currentDepth + 1, aI, ref currentMove);
			if (board.currentTeam() == player)
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