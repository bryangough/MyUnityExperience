using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadialGravity : MonoBehaviour 
{	
	public static float range = 1000;
	
	void FixedUpdate () 
	{
		Collider2D[] cols  = Physics2D.OverlapCircleAll(transform.position, range); 
		List<Rigidbody2D> rbs = new List<Rigidbody2D>();
		foreach(Collider2D c in cols)
		{
			Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
			if(rb != null && rb != GetComponent<Rigidbody2D>() && !rbs.Contains(rb))
			{
				rbs.Add(rb);
				Vector3 offset = transform.position - c.transform.position;
				rb.AddForce( offset / offset.sqrMagnitude * GetComponent<Rigidbody2D>().mass);
			//	Debug.Log(offset +" "+ offset / offset.sqrMagnitude * rigidbody2D.mass);
			}
		}
	}
}
