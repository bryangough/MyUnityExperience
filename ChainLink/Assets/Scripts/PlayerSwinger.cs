using UnityEngine;
using System.Collections;


public class PlayerSwinger :MonoBehaviour
{
	public Color red;
	public Color green;
	//
	public GameObject shot;
	public GameObject joiner;
	public ClimbingMover climber;
	//
	public int score = 0;
	//
	void fireChain(Vector3 hitObject)
	{
		climber.chains[climber.whichChain].chain.fireChain(hitObject);
	}
	void fireChain()
	{
		climber.chains[climber.whichChain].chain.fireChain();
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
	
	//
	public void combineChains()
	{
		if(climber.chains.Length<2)
			return;
		if(climber.chains[0].currentChain!=null && climber.chains[1].currentChain!=null)
		{
			GameObject standingObject = ObjectPool.instance.GetObjectForType(joiner.name);
			LinkBoth climberShot = standingObject.GetComponent<LinkBoth>();
			climberShot.launch(transform.position,climber.chains[0].currentChain,climber.chains[0].currentLink,climber.chains[1].currentChain,climber.chains[1].currentLink);

			climber.chains[0].currentChain.SeverAtLink(climber.chains[0].currentLink+1);
			climber.chains[1].currentChain.SeverAtLink(climber.chains[1].currentLink+1);
		}
	}
	public void launchClimber()
	{
		if(climber.chains[climber.whichChain].currentChain!=null)
		{
			GameObject standingObject = ObjectPool.instance.GetObjectForType(shot.name);
			Autoclimber climberShot = standingObject.GetComponent<Autoclimber>();
			climberShot.launch(transform.position,climber.chains[climber.whichChain].currentChain,climber.chains[climber.whichChain].currentLink);
		}
	}
	public void releaseChain1()
	{
		climber.releaseRope(0);
	}
	public void releaseChain2()
	{
		climber.releaseRope(1);
	}
	public ChainConstruct getChain()
	{
		if(climber)
		{
			if(climber.chains.Length>0)
			{

			}
		}
		return climber.chains[climber.whichChain];
	}
	public void climbFlag(int num, bool val)
	{
//		Debug.Log(num+" "+val);
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
					if (hit.transform.gameObject.tag == "Box" || hit.transform.gameObject.tag == "Ground")
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
			climber.chains[climber.whichChain].chain.turnDir = turnDir;


			if(Input.GetKey("a"))
			{
				releaseChain1();
			}
			if(Input.GetKey("s"))
			{
				releaseChain2();
			}
			if(Input.GetKeyDown("v"))
			{
				launchClimber();
			}
			if(Input.GetKeyDown("c"))
			{
				combineChains();
			}
			climbFlag(0,Input.GetKey("z"));
			//climbFlag(1,Input.GetKey("x"));
			
			climber.doClimb();
		}
}
