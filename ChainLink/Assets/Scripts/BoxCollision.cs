using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//
public class BoxCollision : MonoBehaviour 
{
	public List<BoxCollision> connected = new List<BoxCollision>();
	private float testtime = 1.0f;
	private float testtimecount;
	//
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(coll.gameObject.tag=="Player")
		{
			//test chain they are holding
		}
		if(coll.gameObject.tag=="Chain")
		{
			if(testtimecount+testtime < Time.timeSinceLevelLoad)
			{
				bool foundone = false;
				//
				testtimecount = Time.timeSinceLevelLoad;
				//
				ChainPiece piece = coll.gameObject.GetComponent<ChainPiece>();
				if(piece!=null && piece.myHead.headBox!= null)
				{
					if(piece.myHead.headBox!=this && piece.myHead.tailBox!=this)
					{
						foundone = true;
						for(int j=0;j<connected.Count;j++)
						{
							if(connected[j] == piece.myHead.headBox || connected[j] == piece.myHead.tailBox)
							{
								//already connect
								foundone = false;
							}
						}
					}
				}
				if(foundone)
				{
					print ("Do Bind "+this.name);
					connected.Add(piece.myHead.headBox);
					piece.myHead.tailBox = this;
					//create link
					GameObject standingObject = ObjectPool.instance.GetObjectForType("KineticJoin");
					if(standingObject!=null)
					{
						JoinToKinectic linker = standingObject.GetComponent<JoinToKinectic>();
						linker.combine(GetComponent<Rigidbody2D>(), coll.gameObject.GetComponent<Rigidbody2D>(), coll);
					}
					//
				}
			}
		}
	}
	void Start()
	{
		testtimecount = -1;
		connected.Add(this);
	}
	/*void Update()
	{
		if(testtimecount>-1)
		{

		}
	}*/

	//
	/*
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
	*/
	// this forms links
	//	what about direct test?
	/*					for(int i=0;i<piece.myHead.connected.Count;i++)
					{
						notinthislist = true;
						for(int j=0;j<connected.Count;j++)
						{
							if(connected[j] == piece.myHead.connected[i])
							{
								notinthislist = false;
							}
						}
						if(notinthislist)
						{
							//add to each others lists?
							//would a global 2d array be better for this?
							connected.Add(piece.myHead.connected[i]);
							piece.myHead.connected[i].connected.Add(this);

							foundone = true;
						}
					}*/
}
