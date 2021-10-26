using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorConnection : NetworkManager
{
    /// <summary>
    /// This is invoked when the host is started
    /// </summary>
    public override void OnStartHost()
    {
        Debug.Log("Host starts");
        base.OnStartServer();
    }

    /// <summary>
    /// This is invoked when a client joint this server
    /// </summary>
    /// <param name="conn">The connection that involves this network</param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Player " + conn.address + " join the game");
        if (conn.connectionId == 0)
        {
            Debug.Log("host gameobject found");
        }
        base.OnServerConnect(conn);
    }

    /// <summary>
    /// This is invoked when the server is ready and start listenning to client connection
    /// </summary>
    /// <param name="conn">The connection that involves this network</param>
    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Server ready");
        base.OnServerReady(conn);
    }

    /// <summary>
    /// This is invoked on client side when it connects to the server
    /// </summary>
    /// <param name="conn">The connection that involves this network</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Client ready");
        base.OnClientConnect(conn);
    }
}
