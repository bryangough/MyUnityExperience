using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	public int teamId = 0;
	public Sprite image1;
	public Sprite image2;
	public Square currentSquare;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void setTeam(int teamId)
	{
		this.teamId = teamId;
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		if(sr)
		{
			if( this.teamId ==0 )
			{
				sr.sprite = image1;
			}
			else
			{
				sr.sprite = image2;
			}
		}
	}
}
