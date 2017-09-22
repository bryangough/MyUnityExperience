using UnityEngine;
using System.Collections;

public class HandleSway : MonoBehaviour {
	public float h = 0;
	public float v = 0;
    public float acceleration;
	public Vector2 moveDist;
	/*public Vector2 max = new Vector3(4.0f, 1.0f);
	public Vector2 min = new Vector3(-4.0f, 0.0f);//-0.52f);*/
	public float smoothing = 2.0f;
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
			Vector3 pos = transform.position;
			pos.x = moveDist.x * h;
			pos.y = moveDist.y * v;

			/*
			if( transform.position.x > max.x )
			{
				pos.x = max.x;
			}
			if( transform.position.x < min.x )
			{
				pos.x = min.x;
			}			
			

			if( transform.position.y > max.y )
			{
				pos.y = max.y;
			}
			if( transform.position.y < min.y )
			{
				pos.y = min.y;
			}			
			 */

			transform.position = Vector3.Lerp(transform.position, pos, smoothing * Time.deltaTime);
			//transform.position += new Vector3(10 * Time.deltaTime * h, 0.0f, 0.0f);

			
			
    }
}
