using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceModel
{
	public delegate void AnimationEvent();
  	public event AnimationEvent OnMove;
	public event AnimationEvent OnJump;
	public event AnimationEvent OnKing;
	public event AnimationEvent OnDestroyed;


    public int x;
    public int y;
    public PieceColor color;
    public PieceType type;	
    
	public PieceModel(int x, int y,
        PieceColor color,
        PieceType type = PieceType.MAN)
	{
		this.x = x;
		this.y = y;
		this.color = color;
		this.type = type;
	}

	
	public void Move (MoveModel move, ref PieceModel [,] board)
	{
		board[move.y, move.x] = this;
		board[y, x] = null;
		x = move.x;
		y = move.y;
		// next steps here
		if ( move.isCapture() )
		{
			PieceModel piece = board[move.capture.removeY, move.capture.removeX];
			piece.doRemove(ref board);
			OnJump();
		}
		else
		{
			OnMove();
		}
		//doMove();

		if (type == PieceType.KING)
    		return;

		doKinging(ref board);
	}
	public void doKinging(ref PieceModel [,] board)
	{
		int rows = board.GetLength(0)-1;
		Debug.Log(""+color+" "+y+" "+rows);
		if (color == PieceColor.WHITE && y == 0 || color == PieceColor.BLACK && y == rows)
		{
			type = PieceType.KING;
			OnKing();
			
			//crown.SetActive(true);
		}
	}
	//remove self
	public void doRemove(ref PieceModel [,] board)
	{
		board[y, x] = null;
		OnDestroyed();
	}
	private bool IsMoveInBounds(int x, int y, ref PieceModel[,] board)
	{
		int rows = board.GetLength(0);
		int cols = board.GetLength(1);
		if (x < 0 || x >= cols || y < 0 || y >= rows)
			return false;
		return true;
	}

	public Move[] GetMoves(ref PieceModel[,] board)
	{
		List<Move> moves = new List<Move>();
		if (type == PieceType.KING)
			moves = GetMovesKing(ref board);
		else
			moves = GetMovesMan(ref board);
		return moves.ToArray();
	}
	private List<Move> GetMovesMan(ref PieceModel[,] board)
	{
		// next steps here
		List<Move> moves = new List<Move>(2);
		int[] moveX = new int[] { -1, 1 };
		int moveY = -1;
		if (color == PieceColor.BLACK)
    		moveY = 1;
		foreach (int mX in moveX)
		{
			int nextX = x + mX;
			int nextY = y + moveY;
			
			if (!IsMoveInBounds(nextX, nextY, ref board))
    			continue;
			PieceModel p = board[nextY, nextX];
			if (p != null && p.color == color)
    			continue;
			MoveModel m = new MoveModel();
			m.piece = this;
			if (p == null)
			{
				m.x = nextX;
				m.y = nextY;
			}
			else
			{
				int hopX = nextX + mX;
				int hopY = nextY + moveY;
				Debug.Log(hopX+" "+hopY);
				if (!IsMoveInBounds(hopX, hopY, ref board))
					continue;
				if (board[hopY, hopX] != null)
					continue;
				
				m.x = hopX;
				m.y = hopY;
				m.capture = new Capture(nextX, nextY);
			}
			moves.Add(m);
		}
		return moves;
	}
	private List<Move> GetMovesKing(ref PieceModel[,] board)
	{
		// next steps here
		List<Move> moves = new List<Move>();
		int[] moveX = new int[] { -1, 1 };
		int[] moveY = new int[] { -1, 1 };

		foreach (int mY in moveY)
		{
			foreach (int mX in moveX)
			{
				int nowX = x + mX;
				int nowY = y + mY;

				if (IsMoveInBounds(nowX, nowY, ref board))
				{
					PieceModel p = board[nowY, nowX];
					if (p != null && p.color == color)
    					break;
					MoveModel m = new MoveModel();
					m.piece = this;
					if (p == null)
					{
						m.x = nowX;
						m.y = nowY;
					}
					else
					{
						int hopX = nowX + mX;
						int hopY = nowY + mY;
						if (!IsMoveInBounds(hopX, hopY, ref board))
							break;
						
						m.x = hopX;
						m.y = hopY;
						m.capture = new Capture(nowX, nowY);
					}
					moves.Add(m);
					nowX += mX;
					nowY += mY;
				}
			}                   
		}
		return moves;
	}
	
}
