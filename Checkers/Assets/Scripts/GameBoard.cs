using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//visual of this
public class GameBoard : MonoBehaviour {
	public BoardModel board;
	public Vector2 tileDim;
	public GameObject tile1;
	public GameObject tile2;
	public GameObject piecePrefab;

	public Square [,] map;
	
	//Player input
	public Move[] possibleMoves;
	public Square selectedSquare;
	public bool onlyJumps = false;
	//
	void Start () {
		board = new BoardModel();
		createBoard();
		//
		placePieces();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( board.IsGameOver() )
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
			//handle click on piece to move
			PieceModel selectedPiece = this.board.getPieceAt(square.i, square.j);
			//
			if( selectedPiece != null && !onlyJumps)
			{
				if( selectedPiece.color == board.GetCurrentPlayer() )
				{
					this.resetHighlights();
					possibleMoves = board.getPiecesMoves(selectedPiece);
					if(possibleMoves.Length > 0)
					{
						//can move!
						square.highLight();
						selectedSquare = square;
						foreach (Move mv in possibleMoves)
						{
							MoveModel m = (MoveModel)mv;
							Square otherSquare = this.map[m.y,m.x];
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
						MoveModel m = (MoveModel)mv;
						Square otherSquare = this.map[m.y,m.x];
						if(square == otherSquare)
						{
							this.resetHighlights();
							//do move
							board.movePiece(m);
							//
							if( m.isCapture() )
							{
								Move[] jumpMoves = board.getPiecesMoves(m.piece);
								List<Move> tempJumpMoves = new List<Move>();
								if(jumpMoves.Length > 0)
								{
									foreach (Move secondJumpMove in jumpMoves)
									{
										MoveModel nextJump = (MoveModel)secondJumpMove;
										if( nextJump.isCapture() )
										{
											tempJumpMoves.Add(nextJump);
											Square highlightSquare = this.map[nextJump.y, nextJump.x];
											highlightSquare.highLightMoveable();
										}
									}
								}
								if( tempJumpMoves.Count>0 )
								{
									possibleMoves = tempJumpMoves.ToArray();
									Debug.Log("possibleMoves"+possibleMoves+" "+tempJumpMoves.Count+" "+tempJumpMoves);
									onlyJumps = true;
									Square successSquare = this.map[m.y,m.x];
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
	public void resetPlayer()
	{
		onlyJumps = false;
		possibleMoves = null;
		board.SwitchPlayer();								
		this.resetHighlights();
	}
	public bool isDiagonal(Square from, Square to)
	{
		return false;
	}
	//Places pieces based on pieces model
	public void placePieces()
	{
		//buildPiece
		List<PieceModel> pieces = board.getPieces();
		foreach(PieceModel piece in pieces)
		{
			//should these be saved somewhere?
			Piece.buildPiece(piece, this.gameObject, piecePrefab);
		}
	}
	public void createBoard()
	{
		float hexagonWidth = tileDim.x/100.0f*0.6f;
		float hexagonHeight = tileDim.y/100.0f*0.6f;
		//
		//map = new Tile[Width, Height];
		GameObject matIndex = tile1;
		map = new Square[board.size, board.size];
		for (int i = 0; i < board.size; i++)
		{
			matIndex = (matIndex == tile1) ? tile2 : tile1;
			for (int j = 0; j < board.size; j++)
			{
				matIndex = (matIndex == tile1) ? tile2 : tile1;				
				GameObject tile = (GameObject)Instantiate(matIndex, Vector3.zero, Quaternion.identity);
				tile.transform.parent = gameObject.transform;
				float hexagonX = i;
				float hexagonY = -j;
				tile.transform.localPosition = new Vector3(hexagonX,hexagonY, 0);//-i*0.01f)
				Square square = tile.GetComponent<Square>();
				map[j,i] = square;
				square.j = i;
				square.i = j;
			}
		}
		gameObject.transform.Translate(new Vector3(-hexagonWidth * board.size / 2.0f,  hexagonHeight * board.size / 2.0f, 0));
	}
	public void resetHighlights()
	{
		for (int i = 0; i < board.size; i++)
		{
			for (int j = 0; j < board.size; j++)
			{
				map[j,i].resetHighlight();
			}
		}
	}
	void OnRectTransformDimensionsChange()
	{
		Debug.Log("change");
	}
}
