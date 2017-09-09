using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour {
	public float h = 0;
	public float v = 0;
	float other = 0;
	float angle = 0;
	public Vector3 gravDir;

	public bool changeGravity = true;

	public float gravity()
	{
		return 0.0f;
		//return Physics.gravity
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
	}
}
