using UnityEngine;
using System.Collections;

public class LinkBoth : MonoBehaviour 
{
	public bool isClimber = false;
	bool isActive = false;
	
	//@script RequireComponent(ClimbingMover);
	ClimbingMover climber;
	void Awake () {
		climber = GetComponent<ClimbingMover>();
	}
	public void launch(Vector2 pos, ChainTopHit chain, int linkpos,ChainTopHit chain2, int linkpos2)
	{
		transform.position = pos;
		isActive = isClimber;
		//
		climber.chains[0].climb = isClimber;
		climber.catchRope(chain, linkpos);
		//
		climber.chains[1].climb = isClimber;
		climber.catchRope(chain2, linkpos2);
	}
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(!isActive)
			return;
		if (coll.gameObject.tag == "Box")
		{
			//Debug.Log("BANG! - show explosion. Place this back into pool.");
			isActive = false;
			//stop collision
			//ObjectPool.instance.PoolObject(gameObject);
			climber.chains[0].currentChain.SeverAtLink(climber.chains[0].currentLink+1);
			climber.chains[1].currentChain.SeverAtLink(climber.chains[1].currentLink+1);
		}
	}
	void Update () 
	{
		if(isActive)
		{
			climber.doClimb();
			//snip off extra?
		}
	}
}
