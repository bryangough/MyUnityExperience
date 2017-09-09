using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChainConstruct 
{
	public ChainLauncher chain;
	public ChainTopHit currentChain;
	public int currentLink;
	public HingeJoint2D myHinge;
	public bool climb;
}