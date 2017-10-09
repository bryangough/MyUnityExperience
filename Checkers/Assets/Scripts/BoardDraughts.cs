using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardDraughts : Board
{
    public int size = 8;
    public int numPieces = 12;
    public GameObject prefab;
    protected PieceDraughts[,] board;
	public GameBoard gameBoard;
	public Move[] possibleMoves;
	public Square selectedSquare;
	void Awake()
	{
		board = new PieceDraughts[size, size];
	}
	void Start()
	{
		// initialization and board set up
		// your implementation may vary
		PieceDraughts pd = prefab.GetComponent<PieceDraughts>();
		if (pd == null)
		{
			Debug.LogError("No PieceDraught component detected");
			return;
		}
		gameBoard.createBoard();
		int i;
		int j;
		player = 1;
		int piecesLeft = numPieces;
		for (i = 0; i < size; i++)
		{
			if (piecesLeft == 0)
				break;
			int init = 0;
			if (i % 2 != 0)
				init = 1;
			for (j = init; j < size; j+=2)
			{
				if (piecesLeft == 0)
					break;
				PlacePiece(j, i);
				piecesLeft--;
			}
		}
		//
		player = 0;
		piecesLeft = numPieces;
		for (i = size - 1; i >= 0; i--)
		{
			if (piecesLeft == 0)
				break;
			int init = 0;
			if (i % 2 != 0)
				init = 1;
			for (j = init; j < size; j+=2)
			{
				if (piecesLeft == 0)
					break;
				PlacePiece(j, i);
				piecesLeft--;
			}
		}
		//player = 1;
	}

// 	Vector3 touchPosWorld;
//     TouchPhase touchPhase = TouchPhase.Began;
	void Update () 
	{
		if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position), Vector2.zero);
			if(hit.collider != null)
			{
				Debug.Log ("Touched it");
			}
		}
		if( Input.GetMouseButtonDown(0) )
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null)
			{
				Square square = hit.collider.gameObject.GetComponent<Square>();
				if( square.currentPiece != null )
				{
					PieceDraughts selectedPiece = square.currentPiece;
					selectedSquare = square;
					if( selectedPiece.color == PieceColor.BLACK && player == 1 || selectedPiece.color == PieceColor.WHITE && player == 0)
					{
						gameBoard.resetHighlights();
						possibleMoves = selectedPiece.GetMoves(ref board);
						if(possibleMoves.Length > 0)
						{
							//can move!
							square.highLight();
							foreach (Move mv in possibleMoves)
							{
								MoveDraughts m = (MoveDraughts)mv;
								Square otherSquare = gameBoard.map[m.y,m.x];
								otherSquare.highLightMoveable();
							}
						}
						else
						{
							//reset
						}
					}
				}
				else
				{
					if(possibleMoves != null)
					{
						foreach (Move mv in possibleMoves)
						{
							MoveDraughts m = (MoveDraughts)mv;
							Square otherSquare = gameBoard.map[m.y,m.x];
							if(square == otherSquare)
							{
								PieceDraughts selectedPiece = selectedSquare.currentPiece;
								//do move
								selectedPiece.Move( m, ref board);
								selectedSquare.currentPiece = null;
								gameBoard.map[m.y,m.x].currentPiece = selectedPiece;
								//reset
								//if success, find if anymore success
								if(m.success)
								{
									Move[] jumpMoves = selectedPiece.GetMoves(ref board);
									List<Move> tempJumpMoves = new List<Move>();
									if(possibleMoves.Length > 0)
									{
										List<Move> moves = new List<Move>();
										foreach (Move secondJumpMove in possibleMoves)
										{
											MoveDraughts nextJump = (MoveDraughts)secondJumpMove;
											if(nextJump.success )
											{
												tempJumpMoves.Add(nextJump);
											}
										}
									}
									if(tempJumpMoves.Count>0)
									{
										possibleMoves = tempJumpMoves.ToArray();
									}
								}
								break;
							}
						}
					}
				}
			}
		}
	}
	public void resetPlayer()
	{
		possibleMoves = null;
		SwitchPlayer();								
		gameBoard.resetHighlights();
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
		GameObject go = GameObject.Instantiate(prefab);
		go.transform.position = pos;
		PieceDraughts p = go.GetComponent<PieceDraughts>();
		p.transform.SetParent(gameObject.transform, false);
		//?
		PieceColor color = PieceColor.WHITE;
		if (player == 1)
			color = PieceColor.BLACK;
		p.Setup(x, y, color);
		board[y, x] = p;
		gameBoard.map[y,x].currentPiece = p;
	}
	public override float Evaluate()
	{
		PieceColor color = PieceColor.WHITE;
		if (player == 1)
			color = PieceColor.BLACK;
		return Evaluate(color);
	}
	public override float Evaluate(int player)
	{
		PieceColor color = PieceColor.WHITE;
		if (player == 1)
			color = PieceColor.BLACK;
		return Evaluate(color);
	}
	private float Evaluate(PieceColor color)
	{
		float eval = 1f;
		float pointSimple = 1f;
		float pointSuccess = 5f;

		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		int i;
		int j;

		for (i = 0; i < rows; i++)
		{
			for (j = 0; j < cols; j++)
			{
				PieceDraughts p = board[i, j];
				if (p == null)
					continue;
				if (p.color != color)
					continue;
				Move[] moves = p.GetMoves(ref board);
				foreach (Move mv in moves)
				{
					MoveDraughts m = (MoveDraughts)mv;
					if (m.success)
						eval += pointSuccess;
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
				PieceDraughts p = board[i, j];
				if (p == null)
					continue;
				moves.AddRange(p.GetMoves(ref board));
			}
		}
		return moves.ToArray();
	}

	public int SwitchPlayer()
	{
		if(player == 1)
			player = 0;
		else
			player = 1;
		EventManager.TriggerEvent ("PlayerChange");
		//update change
		return player;
	}
}