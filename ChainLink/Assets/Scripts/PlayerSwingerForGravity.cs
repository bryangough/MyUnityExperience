using UnityEngine;
using System.Collections;


public class PlayerSwingerForGravity :MonoBehaviour
{
	ClimbingMover climber;
	//
	int whichChain = 0;
	public int score = 0;
	
	void fireChain(Vector3 hitObject)
	{
		Rigidbody2D lastObject = climber.chains[whichChain].chain.fireChain(hitObject);
	}
	void fireChain()
	{
		Rigidbody2D lastObject = climber.chains[whichChain].chain.fireChain();
	}
	public void catchRope(Rigidbody2D lastObject)
	{
		if(lastObject!=null)
		{
			climber.chains[whichChain].myHinge.enabled = true;
			climber.chains[whichChain].myHinge.connectedBody = lastObject;
			climber.chains[whichChain].currentLink = climber.chains[whichChain].chain.currentChain.chainPieces.Count;
			
			whichChain++;
			if(whichChain>1)
				whichChain = 0;
		}
	}

	public float gravity()
	{
		return 0.0f;
		//return Physics.gravity
	}

	float h = 0;
	float v = 0;
	float other = 0;
	float angle = 0;
	Vector3 gravDir;
	bool fireRestraint = false;
	public bool changeGravity = true;

	public string getAccel
	{

		get
		{
			return h+"\n"+v+"\n"+other;
		}
	}
	
	void Update () 
	{

		if(changeGravity)
		{
		#if UNITY_IPHONE && !UNITY_EDITOR
			h = Input.acceleration.x;
			v = Input.acceleration.y;
			other = Input.acceleration.z;
		#else
			h = Input.GetAxis("Horizontal"); 
			v = Input.GetAxis("Vertical");
		#endif
			//
			gravDir = new Vector3(h,v,0);
			//angle = Vector3.Angle(gravDir, Vector3.down);
			if(h!=0||v!=0)
			{
				Physics2D.gravity = gravDir.normalized * 5;
			}
		}

		Vector3 pointTouched = Vector3.zero;
		
		//
	#if UNITY_IPHONE && !UNITY_EDITOR
		if(Input.touchCount > 0 && !fireRestraint)
		{
			Touch touch = Input.GetTouch(0);
			pointTouched = Camera.main.ScreenToWorldPoint(touch.position);
	#else
	   	if(Input.GetButton("Fire1") && !fireRestraint)
		{
			pointTouched = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	#endif
			RaycastHit2D hit = Physics2D.Raycast(pointTouched, Vector2.zero);
			if (hit.transform!=null)
			{
				if (hit.transform.gameObject.tag == "Box")
				{
					fireRestraint = true;
					fireChain(pointTouched);
				}
			}
		}
	#if UNITY_IPHONE && !UNITY_EDITOR
		if(Input.touchCount == 0)
	#else
		if(!Input.GetButton("Fire1"))
	#endif
		{
			fireRestraint = false;
		}
		if(Input.GetButtonDown("Jump"))
		{
			fireRestraint = true;
			fireChain();
		}
		GetComponent<Rigidbody2D>().AddForce (transform.right*Input.GetAxis ("Horizontal")*1000);
		//rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, 4);

		float turnDir = Input.GetAxis("Vertical");
		climber.chains[whichChain].chain.turnDir = turnDir;

		if(Input.GetKey("a"))
		{
			releaseChain1();
		}
		if(Input.GetKey("s"))
		{
			releaseChain2();
		}

		if(Input.GetKey("c"))
		{
			combineChains();
		}
		climbFlag(0,Input.GetKey("z"));
		climbFlag(1,Input.GetKey("x"));

		climber.doClimb();
	}
	public void combineChains()
	{

		
	}
	public void launchClimber()
	{
	}
	public void releaseChain1()
	{
		climber.chains[0].myHinge.enabled = false;
	}
	public void releaseChain2()
	{
		climber.chains[1].myHinge.enabled = false;
	}
	public void climbFlag(int num, bool val)
	{
		climber.chains[num].climb = val;
	}
	//
	public void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Coin")
		{
				score++;
		}
		
	}
}
