using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorConnection : NetworkManager
{
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Player " + conn.address + " join the game");
        base.OnServerConnect(conn);
    }
}
