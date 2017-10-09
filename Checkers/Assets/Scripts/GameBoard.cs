using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	BoardDraughts board;
	public int width = 8;
	public int height = 8;
	public Vector2 tileDim;
	public GameObject tile1;
	public GameObject tile2;
	public GameObject gamePiece;

	public Square [,] map;
	// Use this for initialization
	void Start () {
		//createBoard();
		//placePieces();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public bool isDiagonal(Square from, Square to)
	{
		return false;
	}
	
	public void createBoard()
	{
		float hexagonWidth = tileDim.x/100.0f*0.6f;
		float hexagonHeight = tileDim.y/100.0f*0.6f;
		//
		//map = new Tile[Width, Height];
		GameObject matIndex = tile1;
		map = new Square[width, height];
		for (int i = 0; i < width; i++)
		{
			matIndex = (matIndex == tile1) ? tile2 : tile1;
			for (int j = 0; j < height; j++)
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
		gameObject.transform.Translate(new Vector3(-hexagonWidth * width / 2.0f,  hexagonHeight * height / 2.0f, 0));
	}
	public void resetHighlights()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				map[j,i].resetHighlight();
			}
		}
	}

	/*public void placePieces()
	{
		for(var y=0;y<3;y++)
		{
			for(var x=0;x<4;x++)
			{
				int rem = y%2;
				GameObject piece = (GameObject)Instantiate(gamePiece, Vector3.zero, Quaternion.identity);
				piece.transform.parent = gameObject.transform;
				piece.transform.position = map[x+x+rem,y].transform.position;
			}
		}	
	}*/
}
