using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
	public int x;
	public int y;
	public SquareModel location;

	// Use this for initialization
	SpriteRenderer spriteRenderer;
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public Team team
	{
    	get { return location.team; }
    	set { location.team = value; }
	}
	/*public bool isEmpty()
	{
		if( currentPiece == null)
		{
			return true;
		}
		return false;
	}*/
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
