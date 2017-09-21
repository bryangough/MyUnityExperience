using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameHandler : MonoBehaviour {

	public GameObject tilePrefab;
	
	public int numberOfPairs = 2;
	public int width;
	public int height;
	public Vector2 offset;
	public Vector2 cardDimensions;
	public CardHandler [] cards;
	public CardHandler firstCard;
	public CardHandler secondCard;
	public bool startFlipped = false;
	// Use this for initialization
	void Start () {
		createCards();
	}
	

	public CardModel [] cardPossibleValues;
	public CardModel [] pairArray;
	
	// Update is called once per frame
	void Update () {
		
	}
	public void createCards()
	{
		cards = new CardHandler[width * height];

		int numPairs = (width * height) /2;
		pairArray = new CardModel[width*height];
		for(int x=0;x<numPairs;x++)
		{
			pairArray[x] = cardPossibleValues[x];
			pairArray[x+numPairs] = cardPossibleValues[x];
		}
		Shuffle(pairArray);

		//
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject cardGameObject = (GameObject)Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
				CardHandler card = cardGameObject.GetComponent<CardHandler>();
				//Canvas elements must be part of a Canvas chain
				card.transform.parent = this.transform;
				card.transform.localPosition = new Vector3(i*cardDimensions.x+offset.x,j*cardDimensions.y+offset.y, 0);//-i*0.01f);	
				card.gameHandler = this;
				card.setModel( pairArray[i*width+j] );

				if(startFlipped)
				{
					card.startFlipped();
				}
				//card.model = pairArray[i*width+j];
				cards[i*width+j] = card;
			}
		}
		
	}

	public void Shuffle(CardModel[] decklist) {
		CardModel tempCard;
         for (int i = 0; i < decklist.Length; i++) {
             int rnd = Random.Range(0, decklist.Length);
             tempCard = decklist[rnd];
             decklist[rnd] = decklist[i];
             decklist[i] = tempCard;
         }
     }
	public bool flipCard(CardHandler card)
	{
		if(!firstCard)
		{
			firstCard = card;
			return true;
		}
		if(!secondCard)
		{
			secondCard = card;
			if( firstCard.compare(secondCard) )
			{
				firstCard.isMatched = true;
				secondCard.isMatched = true;
				//success
				//disable their flip
				

				firstCard = null;
				secondCard = null;
				
				if( isWon() )
				{
					Debug.Log("Won All!");
					//won sound?
					StartCoroutine(doWin());
				}
			}
			else
			{
				StartCoroutine(doWrongPair());
				//error noise, flip back, delay

			}
			return true;
		}
		return false;
	}
	IEnumerator doWin()
  	{
		//Wait til word is read
		yield return new WaitForSeconds(1);
		//play win Sound
		yield return new WaitForSeconds(2);
		Debug.Log("reset");
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
		//reset
  	}
	IEnumerator doWrongPair()
  	{
		//Make wrong sound
		//Shake cards
		yield return new WaitForSeconds(2);
		putEverythingBack();
  	}
	public void putEverythingBack()
	{
		firstCard.flipBack();
		secondCard.flipBack();

		firstCard = null;
		secondCard = null;
	}
	public bool isWon() 
	{
        for (int i = 0; i < cards.Length; i++) 
		{
        	CardHandler card = cards [i];
        	if (card.isMatched == false) 
			{
        		return false;
        	}
		}
		return true;
    }      
}
