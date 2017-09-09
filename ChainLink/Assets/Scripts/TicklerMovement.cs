using UnityEngine;
using System.Collections;

public class TicklerMovement : MonoBehaviour {

	float countertop = 15;
	Vector2 countertoprandom = new Vector2(-5,5);

	Vector3 startPosition;
	float counter = 30;
	float amountFlap = 0.4f;

	void Start () {
		startPosition = transform.localPosition;
		Debug.Log(startPosition);
		counter = countertop;
	}

	void Update () {
		if(counter<0)
		{
			counter = countertop + Random.Range(countertoprandom.x,countertoprandom.y);
			Vector3 rand = Random.insideUnitSphere * amountFlap;// * Vector3.right;
			rand.y = 0;
			rand.z = 0;
			transform.localPosition = startPosition+rand;
		}
		counter-= (Time.deltaTime*30);
	}
}
