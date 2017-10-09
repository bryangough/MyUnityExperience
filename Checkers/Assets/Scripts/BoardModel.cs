using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//model of gameboard
public class BoardModel : Board
{
    public int size = 8;
    public int numPieces = 12;
    protected PieceModel[,] board;
	//public GameBoard gameBoard;
	protected bool gameDone = false;
	//
	public BoardModel()
	{
		board = new PieceModel[size, size];
		player = PieceColor.BLACK;
		createPlayer();
		//
		player = PieceColor.WHITE;
		createPlayer();
		//
		EventManager.TriggerEvent ("PlayerChange");
	}
	void createPlayer()
	{
		int i;
		int piecesLeft = numPieces;
		if(player == PieceColor.BLACK)
		{
			for (i = 0; i < size; i++)
			{
				piecesLeft = createPlayerInner(i, piecesLeft);
			}
		}
		else
		{
			for (i = size - 1; i >= 0; i--)
			{
				piecesLeft = createPlayerInner(i, piecesLeft);
			}
		}
	}
	int createPlayerInner(int i, int piecesLeft)
	{
		if (piecesLeft == 0)
			return 0;
		int init = 0;
		if (i % 2 != 0)
			init = 1;
		for (int j = init; j < size; j+=2)
		{
			if (piecesLeft == 0)
				break;
			PlacePiece(j, i);
			piecesLeft--;
		}
		return piecesLeft;
	}

// 	Vector3 touchPosWorld;
//     TouchPhase touchPhase = TouchPhase.Began;
	
	public bool testWin()
	{
		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		int i;
		int j;

		for (i = 0; i < rows; i++)
		{
			for (j = 0; j < cols; j++)
			{
				PieceModel p = board[i, j];
				if (p == null)
					continue;
				if (p.color == player)
					return false;
			}
		}
		return true;
	}

	public List<PieceModel> getPieces()
	{
		List<PieceModel> listOfPieces = new List<PieceModel>();
		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		int i;
		int j;
		for (i = 0; i < rows; i++)
		{
			for (j = 0; j < cols; j++)
			{
				PieceModel p = board[i, j];
				if (p != null)
					listOfPieces.Add(p);
			}
		}
		return listOfPieces;
	}
	
	/*public void removePiece(int x,int y)
	{
		gameBoard.map[m.y,m.x].currentPiece = null;
		Destroy(board[move.removeY, move.removeX]);
		board[move.removeY, move.removeX] = null;
	}*/
	private void PlacePiece(int x, int y)
	{
		// your own transformations
		// according to space placements
		Vector3 pos = new Vector3();
		pos.x = (float)x;
		pos.y = -(float)y;
		PieceModel piece = new PieceModel(x,y,player);
		board[y, x] = piece;
		//gameBoard.map[y,x].currentPiece = piece;
	}
	public override float Evaluate()
	{
		return Evaluate(player);
	}
	public override float Evaluate(PieceColor color)
	{
		float eval = 1f;
		float pointSimple = 1f;
		float pointCapture = 5f;

		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		int i;
		int j;

		for (i = 0; i < rows; i++)
		{
			for (j = 0; j < cols; j++)
			{
				PieceModel p = board[i, j];
				if (p == null)
					continue;
				if (p.color != color)
					continue;
				Move[] moves = p.GetMoves(ref board);
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
	public override Move[] GetMoves()
	{
		List<Move> moves = new List<Move>();
		int rows = board.GetLength(0);
		int cols = board.GetLength(1);
		int i;
		int j;
		for (i = 0; i < rows; i++)
		{
			for (j = 0; i < cols; j++)
			{
				PieceModel p = board[i, j];
				if (p == null)
					continue;
				moves.AddRange(p.GetMoves(ref board));
			}
		}
		return moves.ToArray();
	}
	public PieceModel getPieceAt(int i, int j)
	{
		PieceModel p = board[i, j];
		return p;
	}
	public void movePiece(MoveModel m)
	{
		Debug.Log("* "+m+" *");
		m.piece.Move( m, ref board);
	}
	public Move[] getPiecesMoves(PieceModel piece)
	{
		return piece.GetMoves(ref board);
	}
	public PieceColor SwitchPlayer()
	{
		if(player == PieceColor.BLACK)
			player = PieceColor.WHITE;
		else
			player = PieceColor.BLACK;
		if( testWin() )
		{
			gameDone = true;
			Debug.Log("game done!");
			if(player == PieceColor.BLACK)
				player = PieceColor.WHITE;
			else
				player = PieceColor.BLACK;
			EventManager.TriggerEvent ("WinGame");
		}			
		else
		{
			EventManager.TriggerEvent ("PlayerChange");
		}
		//update change
		return player;
	}
	public override bool IsGameOver()
	{
		return gameDone;
	}
}