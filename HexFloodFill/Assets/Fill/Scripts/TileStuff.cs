using UnityEngine;
using System.Collections;

public class TileStuff : MonoBehaviour {

	public Sprite[] littlePieces = null;
	//Sprite[] currentPieces = null;
	void Start()
	{
		//Tile parent = gameObject.transform.parent.GetComponent<Tile>();
		//if(parent.doStuff)
		//	refresh ();
		string s = "[";
		for(int i=0;i<littlePieces.Length;i++)
		{
			s+= "\""+littlePieces[i].name+".png\"";
			if(i<littlePieces.Length-1)
				s+=", ";
		}
		s += "]";
		//print (this.name+s);

	}
	public void refresh()
	{
		/*if(currentPieces.Length>0)
		{
			for (int x = 0; x < currentPieces.Length; x++)
			{
				Destroy(currentPieces[x]);
			}
		}*/
		//
		for (int y = 0; y < Random.Range(0,2); y++)
		{
			GameObject tile = new GameObject();
			SpriteRenderer s = tile.AddComponent<SpriteRenderer>();
			s.sprite = littlePieces[Random.Range(0,littlePieces.Length)];
			tile.transform.parent = gameObject.transform;
			//tile.transform.localPosition = new Vector3(Random.Range(-0.3f,0.3f),Random.Range(0.3f,0f),-1);
			tile.transform.localPosition = new Vector3(Random.Range(-0.1f,0.1f),Random.Range(0.1f,0.0f),-1);
		}
		//			                                                      
	}
}
