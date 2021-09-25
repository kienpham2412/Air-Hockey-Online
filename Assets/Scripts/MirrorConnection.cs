using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorConnection : NetworkManager
{
    public override void OnStartHost()
    {
        Debug.Log("Host starts");
        base.OnStartServer();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Player " + conn.address + " join the game");
        base.OnServerConnect(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Server ready");
        base.OnServerReady(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Client ready");
        base.OnClientConnect(conn);
    }
}
