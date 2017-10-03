using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    Rigidbody2D rigidbody2D;
    public override void OnStartLocalPlayer()
    {
        //GetComponent<MeshRenderer>().material.color = Color.red;
    }
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    [Command]
    void CmdSwing()
    {
       	// This [Command] code is run on the server!
        Debug.Log("SWING");
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        Vector2 _velocity = new Vector2(x,y);
        float speed = 5.0f;
        //transform.Translate(x, y,0);
        rigidbody2D.MovePosition( rigidbody2D.position + _velocity * Time.deltaTime * speed );
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Command function is called from the client, but invoked on the server
            CmdSwing();
        }
    }
    /*void FixedUpdate()
	{
		
		//rigidbody2D.velocity = constantSpeed * (rigidbody2D.velocity.normalized);
	}*/
}
