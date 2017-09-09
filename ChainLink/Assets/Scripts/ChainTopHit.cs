using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//
public class ChainTopHit : MonoBehaviour {
	//for creation
	public List<Rigidbody2D> chainPieces = new List<Rigidbody2D>();
	//public List<BoxCollision> connected = new List<BoxCollision>();
	public BoxCollision headBox;
	public BoxCollision tailBox;

	Rigidbody2D objectBefore;
	int generateMore = 0;
	GameObject chainPrefab;
	[HideInInspector]
	public float linkLength = 0.68f;

	//my own
	HingeJoint2D linkHinge;
	DistanceJoint2D linkDistance;
	Rigidbody2D rigidbody2D;
	BoxCollider2D collider;
	public ChainLauncher myChain;

	//
	private bool launched = false;
	private Vector2 flyDirection;
	private Vector2 flyTarget;
	public float flySpeed = 4;
	public bool hit = false;
	public bool homer = false;
	//
	void Awake () {
		linkHinge = GetComponent<HingeJoint2D>();
		linkDistance = GetComponent<DistanceJoint2D>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
	}
	//
	public void SeverAtLink(int link)
	{
		Debug.Log ("remove link "+link);
		if(link > -1 && chainPieces.Count > link)
		{
			Debug.Log ("do sever");
			GameObject severingObject = chainPieces[link].gameObject;
			HingeJoint2D linkHinge = severingObject.GetComponent<HingeJoint2D>();
			linkHinge.connectedBody = null;
			linkHinge.enabled = false;

			DistanceJoint2D distanceJoint = severingObject.GetComponent<DistanceJoint2D>();
			distanceJoint.connectedBody = null;
			distanceJoint.enabled = false;
			// all severed objects need to be destroyed or moved to their own object (this will impact some climbing)
		}
	}
	//
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(hit)
			return;
		//
		if(coll.gameObject.tag=="Ground" || coll.gameObject.tag == "Box")
		{
			/*destroyChain();
			//destroy self
		}
		else if (coll.gameObject.tag == "Box")
		{*/
			hit = true;

			BoxCollision box = coll.gameObject.GetComponent<BoxCollision>();
			if(box!=null)
				headBox = box;

			linkHinge.connectedBody = coll.gameObject.GetComponent<Rigidbody2D>();//linkDistance.connectedBody;
			linkHinge.enabled = true;
			linkDistance.enabled = false;
			linkHinge.connectedAnchor = coll.gameObject.transform.InverseTransformPoint(coll.contacts[0].point);
			rigidbody2D.gravityScale = 1.0f;
			rigidbody2D.mass = 200.0f;
			rigidbody2D.isKinematic = false;
			if(myChain!=null)
			{
				myChain.playerCatch(chainPieces[chainPieces.Count-1]);
			}
			generateMore = 1;
			adjustWeight();
			clearVelocity();
		}
	}
	public void destroyChain()
	{
		for(int i=0;i<chainPieces.Count;i++)
		{
			ObjectPool.instance.PoolObject(chainPieces[i].gameObject);
		}
		hit = false;
		chainPieces.Clear();
	}
	//
	public void Launch(Vector3 pos,Vector2 dir,GameObject chainPrefab,float linkLength,Vector3 target)
	{
		this.chainPrefab = chainPrefab;
		this.linkLength = linkLength;
		//
		generateMore = 2;
		transform.position = pos;
		chainPieces.Clear();
		chainPieces.Add(rigidbody2D);
		objectBefore = rigidbody2D;


		//
		Quaternion rot = Quaternion.FromToRotation(-Vector3.up, dir);
		rot.x = 0;
		rot.y = 0;
		transform.rotation =  rot;
		//
		flyDirection = -dir;
		flyTarget = target;
		hit = false;
		launched= true;
		rigidbody2D.AddForce(flyDirection,ForceMode2D.Impulse);//40000
	}
	public void releaseChain()
	{
		chanceColourOfAll();
		myChain = null;
	}
	public void adjustWeight()
	{
		for(int x=0;x<chainPieces.Count;x++)
		{
			chainPieces[x].mass = 100+5*x;
			//chainPieces[x]
		}
	}
	public void clearVelocity()
	{
		for(int x=0;x<chainPieces.Count;x++)
		{
			chainPieces[x].velocity = Vector2.zero;
			//chainPieces[x]
		}
	}
	public void chanceColourOfAll()
	{
		for(int x=0;x<chainPieces.Count;x++)
		{
			SpriteRenderer sprite = chainPieces[x].gameObject.GetComponentInChildren<SpriteRenderer>();
			sprite.color = Color.grey;
		}
	}
	public Rigidbody2D CreateLink(string name, Vector3 pos, Vector2 dir, Rigidbody2D objectBefore)
	{
		GameObject standingObject = ObjectPool.instance.GetObjectForType(name);
		
		if(standingObject!=null)
		{
			ChainPiece piece = standingObject.GetComponent<ChainPiece>();
			if(piece!=null)
			{
				piece.myHead = this;
			}


			HingeJoint2D linkHinge = standingObject.GetComponent<HingeJoint2D>();
			linkHinge.connectedBody = objectBefore;
			
			DistanceJoint2D distanceJoint = standingObject.GetComponent<DistanceJoint2D>();
			distanceJoint.connectedBody = objectBefore;
			
			//SpringJoint2D springJoint = standingObject.GetComponent<SpringJoint2D>();
			//springJoint.connectedBody = objectBefore;
			
			standingObject.transform.position = pos;
			Rigidbody2D linkRigidBody = standingObject.GetComponent<Rigidbody2D>();
			
			chainPieces.Add(linkRigidBody);
			return linkRigidBody;
		}
		else
		{
			Debug.Log ("something broke::: standingObject --"+standingObject +"--"+name);
		}
		return null;
	}
	//
	void Update()
	{
		if(!launched)
		{
			return;
		}
		if(generateMore!=0 && myChain!=null)
		{
			if(generateMore==1)//this makes 1 more than needed
				generateMore = 0;
			Vector2 testPos = myChain.transform.position;
			Vector2 dir = new Vector2 (testPos.x-chainPieces[chainPieces.Count-1].transform.position.x,testPos.y-chainPieces[chainPieces.Count-1].transform.position.y);
			float mag = dir.magnitude;
			if(mag>linkLength)
			{
				objectBefore = CreateLink(chainPrefab.name, myChain.transform.position, dir, objectBefore);
			}
		}
		//if at max # links stop generating
	}
	void FixedUpdate()
	{
		if(hit)
		{
			return;
			//destroy timer or destroy when released
		}
		else
		{
			if(homer)
			{
				//Vector2 dir = new Vector2 (this.transform.position.x-flyTarget.x,this.transform.position.y-flyTarget.y);
				//dir = dir.normalized * -1;
				//rigidbody2D.AddForce(flyDirection*100000,ForceMode2D.Force);
				//force the shot to hit the target
				rigidbody2D.MovePosition(rigidbody2D.position + flyDirection * Time.deltaTime * 20);
			}
			else
			{
				rigidbody2D.AddForce(flyDirection*100000,ForceMode2D.Force);
			}
		}
	}
}
