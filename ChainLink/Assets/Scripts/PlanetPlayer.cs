using UnityEngine;
using System.Collections;

public class PlanetPlayer : MonoBehaviour {

	public float maxGravDist = 4.0f;
	public float maxGravity = 35.0f;

	//

	public Planet planet1;
	public Planet planet2;

	//

	public LineRenderer lineDrawer1;
	public LineRenderer lineDrawer2;

	//

	float gravityForce = 10;

	Planet touchingPlanet;



	bool onPlanet = false;

	GameObject[] planets;

	bool left = false;
	bool right = false;
	bool space = false;
	public Transform feet;
	void Start () 
	{
		planets = GameObject.FindGameObjectsWithTag("Planet");


		if(planet1!=null)
		{
			//Planet planet = other.GetComponent<Planet>() as Planet;
			Vector2 direction =  this.transform.position - planet1.transform.position;
			direction -= planet1.planetCenter;
			Vector2 force = direction.normalized * gravity;
			Vector2 stuff = transform.position+feet.localPosition - transform.up;
			GetComponent<Rigidbody2D>().AddForceAtPosition(force, stuff, ForceMode2D.Impulse);
		}
		if(planet2!=null)
		{
			//Planet planet = other.GetComponent<Planet>() as Planet;
			Vector2 direction =  this.transform.position - planet1.transform.position;
			direction -= planet2.planetCenter;
			Vector2 force = direction.normalized * gravity;
			Vector2 stuff = transform.position+feet.localPosition - transform.up;
			GetComponent<Rigidbody2D>().AddForceAtPosition(force, stuff, ForceMode2D.Impulse);
		}
	}

	void Update()
	{
		if (Input.GetKey("left") || left)
		{
			left = true;
		}
		
		if (Input.GetKey("right") || right)
		{
			right = true;
		}

		if(Input.GetButton("Jump") || space)
		{
			space = true;
		}
	}
	float moveForce = 4;
	Vector3 firstClick;
	Vector3 spot2;

	//
	void FixedUpdate () 
	{
		firstClick = transform.position+feet.localPosition;
		firstClick.z = 0;
		lineDrawer1.enabled = true;
		spot2 = planet1.planetCenter;//camera.ScreenToWorldPoint( Input.mousePosition );
		spot2.z = 0;
		lineDrawer1.SetPosition(0, firstClick);
		lineDrawer1.SetPosition(1, spot2);


		firstClick = transform.position+feet.localPosition;
		firstClick.z = 0;
		lineDrawer2.enabled = true;
		spot2 = planet2.planetCenter;//camera.ScreenToWorldPoint( Input.mousePosition );
		spot2.z = 0;
		lineDrawer2.SetPosition(0, firstClick);
		lineDrawer2.SetPosition(1, spot2);
		/*
		if(planet1!=null)
		{
			//Planet planet = other.GetComponent<Planet>() as Planet;
			Vector2 direction =  this.transform.position - planet1.transform.position;
			direction -= planet1.planetCenter;
			Vector2 force = direction.normalized * gravity;
			Vector2 stuff = transform.position+feet.localPosition - transform.up;
			rigidbody2D.AddForceAtPosition(force, stuff, ForceMode2D.Force);
		}
		if(planet2!=null)
		{
			//Planet planet = other.GetComponent<Planet>() as Planet;
			Vector2 direction =  this.transform.position - planet1.transform.position;
			direction -= planet2.planetCenter;
			Vector2 force = direction.normalized * gravity;
			Vector2 stuff = transform.position+feet.localPosition - transform.up;
			rigidbody2D.AddForceAtPosition(force, stuff, ForceMode2D.Force);
		}
		*/
		if(space)
		{
			Debug.Log ("up");
			Vector2 force = this.transform.up * moveForce * 3;
			Vector2 stuff = transform.position+feet.localPosition;
			GetComponent<Rigidbody2D>().AddForceAtPosition(force, stuff, ForceMode2D.Force);
		}
		if(onPlanet)
		{
			if(right)
			{
				Vector2 force = this.transform.right * moveForce;
				Vector2 stuff = transform.position+feet.localPosition;
				GetComponent<Rigidbody2D>().AddForceAtPosition(force, stuff, ForceMode2D.Force);
			}
			if(left)
			{
				Vector2 force = this.transform.right * moveForce * -1;
				Vector2 stuff = transform.position+feet.localPosition;
				GetComponent<Rigidbody2D>().AddForceAtPosition(force, stuff, ForceMode2D.Force);
			}
		}
		else
		{
			if(right)
			{
				GetComponent<Rigidbody2D>().AddTorque(5);
			}
			if(left)
			{
				GetComponent<Rigidbody2D>().AddTorque(-5);
			}
		}
		left = false;
		right = false;
		space = false;
	}



	float gravity = -10;
	
	void OnTriggerStay2D(Collider2D other) {
	/*	if(onPlanet)
		{
			Planet planet = other.GetComponent<Planet>() as Planet;
			Vector2 direction =  this.transform.position - other.transform.position;
			direction -= planet.planetCenter;
			Vector2 force = direction.normalized * gravity;
			Vector2 stuff = transform.position+feet.localPosition - transform.up;
			rigidbody2D.AddForceAtPosition(force,stuff, ForceMode2D.Force);
		}
		else
		{
			Planet planet = other.GetComponent<Planet>() as Planet;
			Vector2 direction =  this.transform.position - other.transform.position;
			direction -= planet.planetCenter;
			Vector2 force = direction.normalized * gravity;
			Vector2 stuff = transform.position+feet.localPosition - transform.up;
			rigidbody2D.AddForceAtPosition(force, stuff, ForceMode2D.Force);
		}*/
	}

	void OnCollisionBegin2D(Collision2D coll) {
		if (coll.gameObject.tag == "Planet")
		{ 
			//onPlanet = true;
		}
	}
	void OnCollisionEnd2D(Collision2D coll) {
		if (coll.gameObject.tag == "Planet")
		{
			onPlanet = false;
		}
	}

}
