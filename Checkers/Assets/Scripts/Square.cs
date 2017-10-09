using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

	public PieceDraughts currentPiece;
	public GameBoard board;
	public int i;
	public int j;
	// Use this for initialization
	SpriteRenderer spriteRenderer;
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
	public void highLight()
	{
		spriteRenderer.color = Color.red;
	}
	public void highLightMoveable()
	{
		spriteRenderer.color = Color.green;
	}
	public void resetHighlight()
	{
		spriteRenderer.color = Color.white;
	}
	/*void OnMouseDown()
	{
		GameObject touchedObject = this.gameObject;
        Debug.Log("Touched " + touchedObject.transform.name);
	} */
}
