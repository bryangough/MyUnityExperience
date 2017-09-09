using UnityEngine;
using System.Collections;

public class HexFieldBasic : MonoBehaviour {
	public int Width = 10;
	public int Height = 10;
	
	public float HexSideLength = 1.0f;
	public GameObject HexTilePrefab = null;
	void Start()
	{
		GenerateGrid();
	}
	public void GenerateGrid()
	{
		float inradius = (float)(0.5 * Mathf.Sqrt(3) * HexSideLength);
		float spaceBetweenTilesHorizontal = 2.0f * inradius;
		float spaceBetweenTilesVertical = 1.5f * HexSideLength;
		
		if (HexTilePrefab != null)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					GameObject tile = (GameObject)Instantiate(HexTilePrefab, Vector3.zero, Quaternion.identity);
					tile.transform.parent = gameObject.transform;
					tile.transform.localPosition = new Vector3(x * spaceBetweenTilesHorizontal + (y & 1) * inradius,  y * spaceBetweenTilesVertical,y);
				}
			}
		}
		// Center the Grid
		gameObject.transform.Translate(new Vector3(-spaceBetweenTilesHorizontal * Width / 2.0f + inradius,  -spaceBetweenTilesVertical * Height / 2.0f - HexSideLength,0));
	}
}