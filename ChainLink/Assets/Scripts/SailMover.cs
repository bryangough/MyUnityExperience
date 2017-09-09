using UnityEngine;
using System.Collections;

public class SailMover : MonoBehaviour 
{
	public float speed = 2.0f;
	public float turnspeed = 300.0f;
	public Vector3 dir = Vector3.up;

	Vector3 movingTowards;
	void Start () {
		newTarget();
	}
	void Update () {
		Vector3 targetDir = movingTowards - transform.position;

		float step = turnspeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.up, targetDir.normalized, turnspeed*Time.deltaTime, 1);
		Quaternion rot = Quaternion.LookRotation( Vector3.forward, newDir );
		//transform.rotation = rot;
		//
		Debug.DrawRay(transform.position, newDir, Color.red);
		Debug.DrawRay(transform.position, targetDir, Color.blue);
		Debug.DrawRay(transform.position, transform.up, Color.green);
		//
		step = speed * Time.deltaTime;
		//transform.Translate(Vector3.up * Time.deltaTime * speed);
		transform.position = Vector3.MoveTowards(transform.position, movingTowards, step);
		if(targetDir.magnitude<0.3)
		{
			newTarget();
		}

		print(Input.GetAxis("Horizontal"));
		if(Input.GetAxis("Horizontal")>0.1||Input.GetAxis("Horizontal")<-0.1)
		{
			transform.Rotate(0.0f, 0.0f, -Input.GetAxis ("Horizontal") * turnspeed * Time.deltaTime);
		}
	}
	public void newTarget()
	{
		movingTowards = Random.insideUnitSphere * 30;
		movingTowards.z = 0;
	}
}
