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
	
	public CheckerPlayer[] players;
	
	//
	void Awake ()
	{
		for(int x =0;x<players.Length;x++)
		{
			players[x].board = this;
		}
	}
	void Start () {
		//
		board = new BoardModel();
		createBoard();
		//
		placePieces();
		//
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
