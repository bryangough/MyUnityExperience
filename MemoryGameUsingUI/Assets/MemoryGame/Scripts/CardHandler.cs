using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour {
	
	public int value;
	public Animator cardAnimator;
	public Text cardText;
	public GameHandler gameHandler;
	public CardModel model;
	public bool isMatched = false;
	// Use this for initialization
	void Start () {
		//cardAnimator 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startFlipped(){
		StartCoroutine(doShowFlipped());
	}

	IEnumerator doShowFlipped()
  	{
		cardAnimator.SetBool("isFlipped", true);
		yield return new WaitForSeconds(3);
		cardAnimator.SetBool("isFlipped", false);
  	}
	public void doFlip(){
		if(	gameHandler.flipCard(this) )
		{
			cardAnimator.SetBool("isFlipped", true);
		}
	}
	public void setModel(CardModel model)
	{
		this.model = model;
		//
		if(this.model!=null)
		{
			setText(model.displayText);
		}
	}

	public void flipBack()
	{
		cardAnimator.SetBool("isFlipped", false);
	}
	public void setText(string text)
	{
		cardText.text = text;
	}
	public bool compare (CardHandler otherCard) {
        return compare (otherCard.model.displayText);
    }
    public bool compare (string otherValue) {
		Debug.Log("compare"+this.model.displayText+" "+otherValue);
        return this.model.displayText == otherValue;
    }

	
}
