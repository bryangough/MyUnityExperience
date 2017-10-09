using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	//events move, king, 
	public GameObject crown;
	
	public Sprite image1;
	public Sprite image2;
	public PieceModel model;
	// Use this for initialization
	public void setup (PieceModel pieceModel) {
		this.model = pieceModel;
		//go.transform.position = pos;
		if( this.model.type == PieceType.MAN )
		{
			crown.SetActive(false);
		}
		setTeam();
		setPosition();
		this.model.OnMove += setPosition;
		this.model.OnDestroyed += destroyMe;
		this.model.OnKing += kingMe;
	}
	void setTeam()
	{
		SpriteRenderer[] sr = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer s in sr)
		{
			if( this.model.color == PieceColor.WHITE )
			{
				s.sprite = image1;
			}
			else
			{
				s.sprite = image2;
			}
		}
	}
	public void setPosition()
	{
		Vector3 pos = this.transform.localPosition;
		pos.x = this.model.x;
		pos.y = -this.model.y;
		this.transform.localPosition = pos;
	}
	// Update is called once per frame

	public void destroyMe()
	{
		Destroy(this.gameObject);
	}
	public void kingMe()
	{
		if( this.model.type == PieceType.MAN )
		{
			crown.SetActive(true);
		}
	}
	void Update () {
		
	}
	public static Piece buildPiece(PieceModel pieceModel, GameObject parentObject, GameObject prefab)
	{
		GameObject go = GameObject.Instantiate(prefab);
		Piece p = go.GetComponent<Piece>();
		p.setup(pieceModel);
		p.transform.SetParent(parentObject.transform, false);
		return p;
	}
	void OnDestroy()
	{
		if(this.model != null)
		{
			this.model.OnMove -= setPosition;
			this.model.OnDestroyed -= destroyMe;
			this.model.OnKing -= kingMe;
		}
	}
}
