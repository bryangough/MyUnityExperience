    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;

    [NetworkSettings(channel = 0, sendInterval = 0.1f)]
    public class PlayerMovement : NetworkBehaviour
    {
        const float moveSpeed = 50f;

        internal struct PlayerState
        {
            internal readonly Vector3 position;
            internal PlayerState(Vector3 pos)
            {
                position = pos;
            }
        };

        [SyncVar(hook = "OnServerStateChanged")]
        PlayerState serverState;

        void Update()
        {
            if (!isLocalPlayer)
                return;

            var horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            var vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            if (horizontal != 0f || vertical != 0f)
            {
                print("Call CmdMove");
                CmdMove(horizontal, vertical);
            }
        }

        [Command]
        void CmdMove(float horizontal, float vertical)
        {
            //print("CmdMove ");
            serverState = new PlayerState( serverState.position + new Vector3(horizontal, 0f, vertical) );
        }

        void OnServerStateChanged(PlayerState newServerState)
        {
            print("OnServerStateChanged "+newServerState.position);
        }
    }
