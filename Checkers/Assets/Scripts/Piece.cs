using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	//events move, king, 
	public GameObject crown;
	
	public Sprite image1;
	public Sprite image2;
	public PieceModel model;
	public float speed = 3.0f;
	//
	public Vector2 movingTo = new Vector2();
	public bool moving = false;

	//
	Animator animator;
	SpriteRenderer spriteRenderer;
	void Start() 
	{
		animator = this.GetComponent<Animator>();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	public void setup (PieceModel pieceModel) {
		this.model = pieceModel;
		setTeam();
		if( this.model.type == PieceType.MAN )
		{
			crown.SetActive(false);
		}
		setPosition();
		//watch Model
		this.model.OnMove += moveTo;
		this.model.OnJump += jumpTo;
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
	
	public void jumpTo()
	{
		animator.Play("Jump", 0);
		moveTo();
		speed = 4.0f;
		spriteRenderer.sortingOrder = 2;
	}
	public void moveTo()
	{
		movingTo.x = this.model.x;
		movingTo.y = -this.model.y;
		moving = true;
		speed = 3.0f;
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
		spriteRenderer.sortingOrder = 1;
		Destroy(this.gameObject, 0.6f);
	}
	public void kingMe()
	{
		if( this.model.type == PieceType.KING )
		{
			crown.SetActive(true);
		}
	}
	
	void Update () {
		if(moving)
		{
			float step = speed * Time.deltaTime;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, movingTo, step);
			if(transform.localPosition.x == movingTo.x && transform.localPosition.y == movingTo.y)
			{
				movingTo = new Vector2();
				moving = false;
			}
		}
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
