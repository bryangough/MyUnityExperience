using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainLauncher : MonoBehaviour 
{
	//[HideInInspector]
	//public ChainConstruct construct;
	[HideInInspector]
	public ClimbingMover player;

	//
	public GameObject chainTopPrefab;
	public ChainTopHit currentChain;
	public GameObject chainPrefab;
	public float linkLength = 0.68f;
	//


	public float maxDistance = 100f;
	
	public float aimSpeed = 2f;
	[HideInInspector]
	public float turnDir;
	public Transform aimPoint;
	//
	public Rigidbody2D fireChain()
	{
		RaycastHit2D hit = Physics2D.Raycast(aimPoint.position, aimPoint.right, maxDistance);
		if(hit!=null)
		{
			return fireChain(hit.point);
		}
		return null;
	}
	public Rigidbody2D fireChain(Vector3 hitObject)
	{
		if(currentChain)
		{
			currentChain.releaseChain();
		}

		Vector2 dir = new Vector2 (this.transform.position.x-hitObject.x,this.transform.position.y-hitObject.y);
		dir = dir.normalized;
		//float mag = dir.magnitude;
		//float distance = mag/linkLength;
		//int num = (int)(distance);

		GameObject standingObject = ObjectPool.instance.GetObjectForType(chainTopPrefab.name);		
		if(standingObject!=null)
		{
			Rigidbody2D linkRigidBody = standingObject.GetComponent<Rigidbody2D>();
			currentChain = standingObject.GetComponent<ChainTopHit>();
			currentChain.myChain = this;		
			currentChain.Launch(this.transform.position,dir,chainPrefab,linkLength,hitObject);
		}

		return null;
	}
	public void playerCatch(Rigidbody2D toCatch)
	{
		if(player)
			player.catchRope(currentChain, toCatch);
	}
	public void updatePointDirection(float dir)
	{
		turnDir = dir;
	}
	public void Update()
	{
		//for aiming
		aimPoint.RotateAround(transform.position, Vector3.forward, -turnDir * aimSpeed);
		turnDir = 0;
	}
}
