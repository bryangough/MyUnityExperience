using UnityEngine;
using System.Collections;

public class VictoryHandler : MonoBehaviour {

	public BoxCollision[] boxes;
	public int numtowin;

	void Start () {
		numtowin = boxes.Length;
	}
	

	void Update () {
		if(boxes[0].connected.Count>=numtowin)
			print ("win");
	}
}
