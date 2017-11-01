using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerPlayer : MonoBehaviour {
	public GameBoard board;
	// Use this for initialization
	//Player input
	public Move[] possibleMoves;
	public bool onlyJumps = false;
	public MoveModel nextModel = null;
	public PieceColor team;
	void Start () {
		//board = GameObject.FindGameObjectsWithTag("GameBoard");
	}
	
	void Update () 
	{
		if(board==null)
			return;
		if( board.board.isGameOver() )
		{
			return;
		}
		if( board.board.getCurrentPlayer() != team)	
			return;
		/*if( nextModel != null)
		{
			testForDoubleJump(nextModel);
			nextModel = null;
		}*/
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
			PieceModel selectedPiece = board.board.getPieceAt(square.i, square.j);
			//
			if( selectedPiece != null && !onlyJumps)
			{
				if( selectedPiece.color == board.board.getCurrentPlayer() )
				{
					board.resetHighlights();
					possibleMoves = board.board.getPiecesMoves(selectedPiece);
					if(possibleMoves.Length > 0)
					{
						//can move!
						square.highLight();
						foreach (Move mv in possibleMoves)
						{
							MoveModel m = (MoveModel)mv;
							Square otherSquare = board.map[m.y,m.x];
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
						Square otherSquare = board.map[m.y,m.x];
						if(square == otherSquare)
						{
							board.resetHighlights();
							//do move
							board.board.movePiece(m);
							//below needs to be moved to after the animation completes
							nextModel = m;
							testForDoubleJump(nextModel);
							break;
						}
					}
				}
			}
		}
	}
	// Update is called once per frame
	public void testForDoubleJump(MoveModel m)
	{
		if( m.isCapture() )
		{
			Move[] jumpMoves = board.board.getPiecesMoves(m.piece);
			List<Move> tempJumpMoves = new List<Move>();
			if(jumpMoves.Length > 0)
			{
				foreach (Move secondJumpMove in jumpMoves)
				{
					MoveModel nextJump = (MoveModel)secondJumpMove;
					if( nextJump.isCapture() )
					{
						tempJumpMoves.Add(nextJump);
						Square highlightSquare = board.map[nextJump.y, nextJump.x];
						highlightSquare.highLightMoveable();
					}
				}
			}
			if( tempJumpMoves.Count>0 )
			{
				possibleMoves = tempJumpMoves.ToArray();
				Debug.Log("possibleMoves"+possibleMoves+" "+tempJumpMoves.Count+" "+tempJumpMoves);
				onlyJumps = true;
				Square successSquare = board.map[m.y,m.x];
				successSquare.highLight();
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
	}
	public void resetPlayer()
	{
		onlyJumps = false;
		possibleMoves = null;
		board.board.SwitchPlayer();								
		board.resetHighlights();
	}
}
