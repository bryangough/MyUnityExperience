using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    WHITE,
    BLACK
};

public enum PieceType
{
    MAN,
    KING
};
public class PieceDraughts : MonoBehaviour
{
    public int x;
    public int y;
    public PieceColor color;
    public PieceType type;
	public Sprite image1;
	public Sprite image2;
    // next steps here

	public void Setup(int x, int y,
        PieceColor color,
        PieceType type = PieceType.MAN)
	{
		this.x = x;
		this.y = y;
		this.color = color;
		this.type = type;
		setTeam();
	}

	void setTeam()
	{
		SpriteRenderer[] sr = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer s in sr)
		{
			if( this.color == PieceColor.WHITE )
			{
				s.sprite = image1;
			}
			else
			{
				s.sprite = image2;
			}
		}
	}
	public void Move (MoveDraughts move, ref PieceDraughts [,] board)
	{
		board[move.y, move.x] = this;
		board[y, x] = null;
		x = move.x;
		y = move.y;
		// next steps here
		if (move.success)
		{
			Debug.Log("remove "+move.removeX+" "+move.removeY);
			PieceDraughts piece = board[move.removeY, move.removeX];
			Debug.Log("delete "+piece);
			Destroy(piece.gameObject);
			board[move.removeY, move.removeX] = null;
		}
		doMove();

		if (type == PieceType.KING)
    		return;

		doKinging(ref board);
	}
	public void doKinging(ref PieceDraughts [,] board)
	{
		int rows = board.GetLength(0);
		if (color == PieceColor.WHITE && y == rows)
			type = PieceType.KING;
		if (color == PieceColor.BLACK && y == 0)
			type = PieceType.KING;
	}
	public void doMove()
	{
		Vector3 pos = this.transform.localPosition;
		Debug.Log(x+" "+y);
		pos.x = x;
		pos.y = -y;
		this.transform.localPosition = pos;
	}

	private bool IsMoveInBounds(int x, int y, ref PieceDraughts[,] board)
	{
		int rows = board.GetLength(0);
		int cols = board.GetLength(1);
		if (x < 0 || x >= cols || y < 0 || y >= rows)
			return false;
		return true;
	}

	public Move[] GetMoves(ref PieceDraughts[,] board)
	{
		List<Move> moves = new List<Move>();
		if (type == PieceType.KING)
			moves = GetMovesKing(ref board);
		else
			moves = GetMovesMan(ref board);
		return moves.ToArray();
	}
	private List<Move> GetMovesMan(ref PieceDraughts[,] board)
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
			PieceDraughts p = board[nextY, nextX];
			if (p != null && p.color == color)
    			continue;
			MoveDraughts m = new MoveDraughts();
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
				m.success = true;
				m.removeX = nextX;
				m.removeY = nextY;
			}
			moves.Add(m);
		}
		return moves;
	}
	private List<Move> GetMovesKing(ref PieceDraughts[,] board)
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

				while (IsMoveInBounds(nowX, nowY, ref board))
				{
					PieceDraughts p = board[nowY, nowX];
					if (p != null && p.color == color)
    					break;
					MoveDraughts m = new MoveDraughts();
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
						m.success = true;
						m.x = hopX;
						m.y = hopY;
						m.removeX = nowX;
						m.removeY = nowY;
					}
					moves.Add(m);
					nowX += mX;
					nowY += mY;
				}
			}                   
		}
		return moves;
	}
	void OnDestroy()
	{

	}
	/*void OnMouseDown()
	{
		Debug.Log("asdf");
		GameObject touchedObject = this.gameObject;
        Debug.Log("Touched " + touchedObject.transform.name);
	} */
}
