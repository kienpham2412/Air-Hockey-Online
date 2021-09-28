using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera playerCam;

    [SerializeField] float zoom = 1.2f;
    private int isHost;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponent<Camera>();
        playerCam.orthographicSize = zoom;

        RotateCamera();
    }

    /// <summary>
    /// Rotate the camera for client gameobject
    /// </summary>
    private void RotateCamera()
    {
        isHost = PlayerPrefs.GetInt("IsHost");
        if(isHost == 0)
        {
            //transform.Rotate(new Vector3(90, 180, 0));
        }
    }
}
