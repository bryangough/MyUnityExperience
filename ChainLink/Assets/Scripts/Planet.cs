using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
	public float planetGravity = 0;

	public Vector2 planetCenter;
	public Transform center;
	//public Transform planetCenter;
	void Start()
	{
		planetCenter = center.position;
	}
	void OnDrawGizmosSelected() {
	//	Gizmos.color = Color.yellow;
	//	Gizmos.DrawWireSphere(planetCenter, 1);
	}


}
