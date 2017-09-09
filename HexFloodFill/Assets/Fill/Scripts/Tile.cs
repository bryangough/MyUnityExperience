using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	/*public enum Skills
	{
		name,
		icon,
		difficulty
	}*/
	public int tileType = 0;
	public GameObject tile = null;
	public bool normalTile = true;

	public int x;
	public int y;
	public bool doStuff = true;
}
