using UnityEngine;
using System.Collections;

public class JoinToKinectic : MonoBehaviour {

	private HingeJoint2D hinge1;
	private HingeJoint2D hinge2;


	void Awake () 
	{
		HingeJoint2D[] hinges = gameObject.GetComponents<HingeJoint2D>();
		hinge1 = hinges[0];
		hinge2 = hinges[1];
	}
	public void combine(Rigidbody2D rigid1, Rigidbody2D rigid2, Collision2D coll)
	{
		//hinge1.connectedAnchor = coll.gameObject.transform.InverseTransformPoint(coll.contacts[0].point);

		hinge1.connectedBody = rigid1;

		hinge2.connectedBody = rigid2;
	}
}
