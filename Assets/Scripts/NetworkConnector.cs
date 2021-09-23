using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkConnector : MonoBehaviour
{
    private NetworkManager networkManager;

    [SerializeField] private int isHost;
    [SerializeField] private string hostIP;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = gameObject.GetComponent<MirrorConnection>();
        Connect();
    }

    /// <summary>
    /// Open a host instance or connect to another host using ip address
    /// </summary>
    private void Connect()
    {
        isHost = PlayerPrefs.GetInt("IsHost");
        hostIP = PlayerPrefs.GetString("HostIP");
        if (isHost == 1)
        {
            networkManager.StartHost();
        }
        else
        {
            networkManager.networkAddress = hostIP;
            networkManager.StartClient();
        }
    }
}
