using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

	public Piece currentPiece;
	public GameBoard board;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isEmpty()
	{
		if( currentPiece == null)
		{
			return true;
		}
		return false;
	}
}
