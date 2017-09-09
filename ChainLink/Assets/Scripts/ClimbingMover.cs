using UnityEngine;
using System.Collections;

public class ClimbingMover : MonoBehaviour 
{
	public float climbSpeed = 1.2f;
	public ChainConstruct[] chains;
	//
	public int whichChain = 0;
	//
	public void Start()
	{
		for(int x=0;x<chains.Length;x++)
		{
			//chains[x].myHinge.enabled = false;
			if(chains[x].chain!=null)
			{
//				print ("set "+x);
				//chains[x].chain.construct = chains[x];
				chains[x].chain.player = this;
			}
		}
	}
	//
	public void catchRope(ChainTopHit currentChain)
	{
		catchRope(currentChain, currentChain.chainPieces[currentChain.chainPieces.Count-1], currentChain.chainPieces.Count-1);
	}
	//
	public void catchRope(ChainTopHit currentChain, Rigidbody2D chainLinkOn)
	{
		catchRope(currentChain, chainLinkOn, currentChain.chainPieces.Count-1);
	}
	//
	public void catchRope(ChainTopHit currentChain, int onLink)
	{
		catchRope(currentChain, currentChain.chainPieces[onLink], onLink);
	}
	public void releaseRope(int whichRope)
	{
		chains[whichRope].myHinge.enabled = false;
		if(chains[whichRope].currentChain!=null)
		{
			chains[whichRope].currentChain.releaseChain();
			chains[whichRope].chain.currentChain = null;
			chains[whichRope].currentChain = null;
		}
	}
	//
	public void catchRope(ChainTopHit currentChain, Rigidbody2D chainLinkOn, int onLink)
	{
		chains[whichChain].myHinge.enabled = true;
		chains[whichChain].myHinge.connectedBody = chainLinkOn;
		chains[whichChain].currentChain = currentChain;
		chains[whichChain].currentLink = onLink;
		whichChain++;
		if(whichChain>chains.Length-1)
			whichChain = 0;

	}
	//need check if launching another?
	public void doClimb()
	{
		for(int x=0;x<chains.Length;x++)
		{
			if(chains[x].climb==false)
				continue;
			if(chains[x].currentChain==null)
				continue;

			Vector2 anchor = chains[x].myHinge.connectedAnchor;
			if(anchor.y>chains[x].currentChain.linkLength)
			{
				chains[x].currentLink--;
				if(chains[x].currentLink>=0)
				{
					//this should be cleaned upCa
					chains[x].myHinge.connectedBody = chains[x].currentChain.chainPieces[chains[x].currentLink];//chains[x].chain.
					anchor.y = 0;
					chains[x].myHinge.connectedAnchor = anchor;
				}
			}
			else
			{
				anchor.y += climbSpeed * Time.deltaTime;
				chains[x].myHinge.connectedAnchor = anchor;
			}
		}
	}
}
