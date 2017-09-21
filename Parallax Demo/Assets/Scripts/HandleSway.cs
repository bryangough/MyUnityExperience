using UnityEngine;
using System.Collections;

public class HandleSway : MonoBehaviour {
	public float h = 0;
	public float v = 0;
    public float acceleration;
	public float limit = 2.0f;
    void Update () 
	{
			#if UNITY_IPHONE && !UNITY_EDITOR
			h = Input.acceleration.x;
			v = Input.acceleration.y;
			acceleration = Input.acceleration.z;
			#else
			h = Input.GetAxis("Horizontal"); 
			v = Input.GetAxis("Vertical");
			#endif

			//
			//float translation = Time.deltaTime * 10 * h;
        	//thisTransform.Translate(0, 0, translation);
			if(transform.position.x>limit)
			{
				transform.position = new Vector3(limit, 0.0f, -10.0f);
			}
			if(transform.position.x<limit*-1)
			{
				transform.position = new Vector3(limit*-1.0f, 0.0f, -10.0f);
			}
			Vector3 pos = transform.position;
			pos.x = limit * h;
			transform.position = pos;
			//transform.position += new Vector3(10 * Time.deltaTime * h, 0.0f, 0.0f);

			
			
    }
}
