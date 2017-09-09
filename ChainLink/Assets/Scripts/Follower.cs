using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
	public Transform objectFollowing;
	public float offset = 0.0f;
	void Start () {
	}
	void Update () 
	{
		Vector3 pos = objectFollowing.position;
		pos.z = this.transform.position.z;
		this.transform.position = pos;
	}
}
