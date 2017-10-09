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

	//Player turn
	public Move[] possibleMoves;
	public Square selectedSquare;
	public bool onlyJumps = false;

	public bool gameDone = false;
	//
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
		//
		player = PieceColor.BLACK;
		createPlayer();
		//
		player = PieceColor.WHITE;
		createPlayer();
		//player = 1;
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
	void Update () 
	{
		if( gameDone )
		{
			return;
		}
		Square square = null;
		//mobile
		if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position), Vector2.zero);
			if(hit.collider != null)
			{
				square = hit.collider.gameObject.GetComponent<Square>();
			}
		}
		//editor
		if( Input.GetMouseButtonDown(0) )
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null)
			{
				square = hit.collider.gameObject.GetComponent<Square>();
			}
		}
		if(square!=null)
		{
			if( square.currentPiece != null && !onlyJumps)
			{
				PieceDraughts selectedPiece = square.currentPiece;
				
				if( selectedPiece.color == player || selectedPiece.color == player)
				{
					gameBoard.resetHighlights();
					possibleMoves = selectedPiece.GetMoves(ref board);
					if(possibleMoves.Length > 0)
					{
						//can move!
						square.highLight();
						selectedSquare = square;
						foreach (Move mv in possibleMoves)
						{
							MoveDraughts m = (MoveDraughts)mv;
							Square otherSquare = gameBoard.map[m.y,m.x];
							otherSquare.highLightMoveable();
						}
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
							gameBoard.resetHighlights();
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
								if(jumpMoves.Length > 0)
								{
									foreach (Move secondJumpMove in jumpMoves)
									{
										MoveDraughts nextJump = (MoveDraughts)secondJumpMove;
										if( nextJump.success )
										{
											tempJumpMoves.Add(nextJump);
											Square highlightSquare = gameBoard.map[nextJump.y, nextJump.x];
											highlightSquare.highLightMoveable();
										}
									}
								}
								if( tempJumpMoves.Count>0 )
								{
									possibleMoves = tempJumpMoves.ToArray();
									Debug.Log("possibleMoves"+possibleMoves+" "+tempJumpMoves.Count+" "+tempJumpMoves);
									onlyJumps = true;
									Square successSquare = gameBoard.map[m.y,m.x];
									successSquare.highLight();
									selectedSquare = successSquare;
									//
								}
								else
								{
									resetPlayer();
								}
							}
							else
							{
								resetPlayer();
							}
							break;
						}
					}
				}
			}
		}
	}
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
				PieceDraughts p = board[i, j];
				if (p == null)
					continue;
				if (p.color == player)
					return false;
			}
		}
		return true;
	}
	public void resetPlayer()
	{
		onlyJumps = false;
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
		p.Setup(x, y, player);
		board[y, x] = p;
		gameBoard.map[y,x].currentPiece = p;
	}
	public override float Evaluate()
	{
		return Evaluate(player);
	}
	public override float Evaluate(int player)
	{
		return Evaluate(player);
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
}