using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//
public class HexFieldBasicWithWater : MonoBehaviour {
	public LoopHandler loopHandler;
	public int startX = 0;
	public int startY = 13;
	public GameObject StartHouse;
	public GameObject TilePrefab;
	public float HexSideLength = 1.0f;

	public GameObject[] HexTilePrefab = null;
	public GameObject[] clickableObject = null;
	//public int mapWidth = 15;
	//public int mapHeight = 15;
	//public GameObject WaterTilePrefab = null;
	public int Width = 18;
	public int Height = 14;

	bool setInitialStart = true;

	public int clicks = 0;
	public int numberOfChoices = 4;
	public static bool pause = false;

	public struct IntVector2
	{
		public int x;
		public int y;

		public IntVector2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		int sqrMagnitude
		{
			get { return x * x + y * y; }
		}
	}

	Tile [,] map;

	void Start()
	{
		if(numberOfChoices>HexTilePrefab.Length)
			numberOfChoices = HexTilePrefab.Length;
		for (int x = numberOfChoices; x < clickableObject.Length; x++)
		{
			clickableObject[x].SetActive(false);
		}
		GenerateGrid();
		pause = false;
	}
	int floodFillStepCount = 0;
	int tempStepCount = 0;
	void Update () {
		if(pause)
		{
			return;
		}
		if (q!= null && q.Count > 0)
		{
			floodFillStepCount ++;
			tempStepCount = floodFillStepCount/3;
			while(tempStepCount>0 && q.Count > 0)
			{
				tempStepCount--;
				floodFillStep();
			}
			bool win = testIfWon();
			if(win)
			{
				loopHandler.showWinScreen();
				//Debug.Log ("win");
			}
			return;
		}
		if(setInitialStart)
		{
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.transform!=null)
				{
					Tile hitObject = hit.transform.GetComponent<Tile>();
					Debug.Log ( "tile "+hitObject.tileType);
					if(hitObject.normalTile)
					{
						startX = hitObject.x;
						startY = hitObject.y;
						//GameObject house = (GameObject)Instantiate(StartHouse, Vector3.zero, Quaternion.identity);
						Vector3 pos = StartHouse.transform.position;
						pos.x = hit.transform.position.x;
						pos.y = hit.transform.position.y;
						StartHouse.transform.position = pos;
						setInitialStart = false;
					}
				}
			}
		}
		//
		if (Input.GetButtonDown ("Fire1")) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.transform!=null)
			{
				Tile hitObject = hit.transform.GetComponent<Tile>();
				Debug.Log ( map[startX,startY].tileType!=hitObject.tileType);
				if(map[startX,startY].tileType!=hitObject.tileType)
				{
					clicks++;
					SetUpFloodFill(new IntVector2(startX,startY),map[startX,startY].tileType,hitObject.tileType);
				}
			}
		}

	}
	public void GenerateGrid()
	{
		float inradius = (float)(0.5 * Mathf.Sqrt(3) * HexSideLength);
		float spaceBetweenTilesHorizontal = 2.0f * inradius;
		float spaceBetweenTilesVertical = 1.5f * HexSideLength-0.15f;
		map = new Tile[Width,Height];
		if (HexTilePrefab != null)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					GameObject tile = (GameObject)Instantiate(TilePrefab, Vector3.zero, Quaternion.identity);
					tile.transform.parent = gameObject.transform;//+ Random.Range(0.0f,0.5f)
					tile.transform.localPosition = new Vector3(x * spaceBetweenTilesHorizontal + (y & 1) * inradius,  y * spaceBetweenTilesVertical,y);
					//
					int random = Random.Range(0,numberOfChoices);
					GameObject tileDesign = (GameObject)Instantiate(HexTilePrefab[random], Vector3.zero, Quaternion.identity);
					tileDesign.transform.parent = tile.transform;
					tileDesign.transform.localPosition = Vector3.zero;
					//
					TileStuff tileStuff = tileDesign.GetComponent<TileStuff>();
					tileStuff.refresh();
					//
					map[x,y] = tile.GetComponent<Tile>();
					map[x,y].tileType = random;
					map[x,y].tile = tileDesign;
					map[x,y].x = x;
					map[x,y].y = y;
				}
			}
		}
		// Center the Grid
		gameObject.transform.Translate(new Vector3(-spaceBetweenTilesHorizontal * Width / 2.0f + inradius+1.0f,  -spaceBetweenTilesVertical * Height / 2.0f - HexSideLength + 0.5f,0));
	}
	private Queue<IntVector2> q;
	int target;
	int replacement;
	void SetUpFloodFill(IntVector2 pt, int target, int replacement)
	{
		q =  new Queue<IntVector2>();
		q.Enqueue(pt);
		this.target = target;
		this.replacement = replacement;
		floodFillStepCount = 0;
	}
	void floodFillStep()
	{
		while(q.Count > 0)
		{
			IntVector2 n = q.Dequeue();
			if(map[n.x,n.y].tileType != target)
			{
				continue;
			}
			//switch tile
			Destroy(map[n.x,n.y].tile);
			GameObject tile = map[n.x,n.y].gameObject;
			//
			GameObject tileDesign = (GameObject)Instantiate(HexTilePrefab[replacement], Vector3.zero, Quaternion.identity);
			tileDesign.transform.parent = tile.transform;
			tileDesign.transform.localPosition = Vector3.zero;

			TileStuff tileStuff = tileDesign.GetComponent<TileStuff>();
			if(map[n.x,n.y].doStuff)
				tileStuff.refresh();
			//
			map[n.x,n.y].tileType = replacement;
			map[n.x,n.y].tile = tileDesign;
			//add neighbors to queue
			if(n.y % 2 == 1)
			{
				//Debug.Log (" y % 2 == 1 "+n.x+" "+n.y );
				addNeighber(q, n.x,n.y-1);
				addNeighber(q, n.x-1,n.y);
				addNeighber(q, n.x,n.y+1);
				addNeighber(q, n.x+1,n.y+1);
				addNeighber(q, n.x+1,n.y);

				addNeighber(q, n.x+1,n.y-1);
				//addNeighber(q, n.x-1,n.y+1);
			}
			else
			{
				//Debug.Log (" y % 2 == 0 "+n.x+" "+n.y );
				addNeighber(q, n.x-1,n.y-1);
				addNeighber(q, n.x-1,n.y);

				//addNeighber(q, n.x+1,n.y-1);
				addNeighber(q, n.x-1,n.y+1);

				addNeighber(q, n.x,n.y+1);
				addNeighber(q, n.x+1,n.y);
				addNeighber(q, n.x,n.y-1);
			}
			return;
		}
	}

	/*
	public void FloodFill(IntVector2 pt, int target, int replacement)
	{
		Queue<IntVector2> q = new Queue<IntVector2>();
		q.Enqueue(pt);
		while (q.Count > 0)
		{
			IntVector2 n = q.Dequeue();
			if(map[n.x,n.y].tileType != target)
			{
				continue;
			}
			//switch tile
			Destroy(map[n.x,n.y].tile);
			GameObject tile = map[n.x,n.y].gameObject;
			//
			GameObject tileDesign = (GameObject)Instantiate(HexTilePrefab[replacement], Vector3.zero, Quaternion.identity);
			tileDesign.transform.parent = tile.transform;
			tileDesign.transform.localPosition = Vector3.zero;
			//
			map[n.x,n.y].tileType = replacement;
			map[n.x,n.y].tile = tileDesign;
			//add neighbors to queue
			if(n.y % 2 == 0)
			{
				addNeighber(q, n.x,n.y-1);
				addNeighber(q, n.x-1,n.y);
				addNeighber(q, n.x+1,n.y);
				addNeighber(q, n.x-1,n.y+1);
				addNeighber(q, n.x+1,n.y+1);
				addNeighber(q, n.x,n.y+1);
			}
			else
			{
				addNeighber(q, n.x,n.y-1);
				addNeighber(q, n.x-1,n.y-1);
				addNeighber(q, n.x+1,n.y-1);
				addNeighber(q, n.x-1,n.y);
				addNeighber(q, n.x+1,n.y);
				addNeighber(q, n.x,n.y+1);
			}
		}
	}
	*/
	bool testIfWon()
	{
		int currentType = map[0,0].tileType;
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if(map[x,y].tileType!=currentType)
					return false;
			}
		}
		return true;
	}
	void addNeighber(Queue<IntVector2> q, int x, int y)
	{
		//Debug.Log ("add: "+x+" "+y);
		if(x > -1 && x < Width && y > -1 && y < Height)
		{
			if(map[x,y]!=null)
			{
				q.Enqueue(new IntVector2(x,y));
			}
		}
	}
}