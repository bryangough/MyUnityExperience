using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {
	public delegate void ChainEvent();
	public static event ChainEvent HitObject, CleanUp;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
