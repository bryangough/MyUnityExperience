using UnityEngine;
using System.Collections;

public class BoardAI
{
	public float Minimax(
			BoardModel board,
			PieceColor player,
			int maxDepth,
			int currentDepth,
			ref Move bestMove)
	{
		if (board.IsGameOver() || currentDepth == maxDepth)
    		return evaluate(player, board);

		bestMove = null;
		float bestScore = Mathf.Infinity;
		if (board.getCurrentPlayer() == player)
    		bestScore = Mathf.NegativeInfinity;

		foreach (Move m in board.getMoves())
		{
			BoardModel b = board.makeMove(m);
			float currentScore;
			Move currentMove = null;
			currentScore = Minimax(b, player, maxDepth, currentDepth + 1, ref currentMove);
			if (board.getCurrentPlayer() == player)
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


	public float evaluate(PieceColor color, BoardModel board)
	{
		float eval = 1f;
		float pointSimple = 1f;
		float pointCapture = 5f;

		int rows = board.board.GetLength(0);
		int cols = board.board.GetLength(1);

		int i;
		int j;

		for (i = 0; i < rows; i++)
		{
			for (j = 0; j < cols; j++)
			{
				PieceModel p = board.board[i, j];
				if (p == null)
					continue;
				if (p.color != color)
					continue;
				Move[] moves = p.GetMoves(ref board.board);
				foreach (Move mv in moves)
				{
					MoveModel m = (MoveModel)mv;
					if ( m.isCapture() )
						eval += pointCapture;
					else
						eval += pointSimple;
				}
			}
		}
		return eval;
	}
}