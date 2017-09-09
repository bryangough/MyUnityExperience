using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ClimbingMover))]
[RequireComponent (typeof (HingeJoint2D))]
public class Autoclimber : MonoBehaviour 
{
	bool isActive = false;

	//@script RequireComponent(ClimbingMover);
	ClimbingMover climber;
	void Awake () {
		climber = GetComponent<ClimbingMover>();
	}
	public void launch(Vector2 pos, ChainTopHit chain, int linkpos)
	{
		transform.position = pos;
		climber.chains[0].climb = true;
		climber.catchRope(chain, linkpos);
		isActive = true;
	}
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(!isActive)
			return;
		if (coll.gameObject.tag == "Box" || coll.gameObject.tag == "Ground")
		{
			Debug.Log("BANG! - show explosion. Place this back into pool.");
			isActive = false;
			//stop collision
			ObjectPool.instance.PoolObject(gameObject);
		}
	}
	void Update () 
	{
		if(isActive)
		{
			climber.doClimb();
		}
	}
}
