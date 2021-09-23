using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    private Vector3 verticalMovement;
    private Vector3 horizontalMovement;

    [SerializeField] private float speed = 20;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    private float xPos, zPos;
    private float xLimitRange = 0.4f;
    private float zLimitRange = 0.75f;
    private float middleBorder = 0f;
    private float upperZLimitRange, lowerZLimitRange;
    private int isHost;
    // Start is called before the first frame update
    void Start()
    {
        isHost = PlayerPrefs.GetInt("IsHost");
        speed = PlayerPrefs.GetFloat("MouseSensitive");
        verticalMovement = Vector3.forward * speed;
        horizontalMovement = Vector3.right * speed;
        if (isHost == 1)
        {
            upperZLimitRange = middleBorder;
            lowerZLimitRange = -zLimitRange;
        }
        else
        {
            upperZLimitRange = zLimitRange;
            lowerZLimitRange = middleBorder;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ControlMovement();
    }

    /// <summary>
    /// Control player's movement and prevent going outside of the game area
    /// </summary>
    private void ControlMovement()
    {
        xPos = transform.position.x;
        zPos = transform.position.z;

        //if (this.isLocalPlayer)
        //{
        //    horizontalInput = Input.GetAxis("Mouse X");
        //    verticalInput = Input.GetAxis("Mouse Y");
        //    transform.Translate(verticalMovement * verticalInput * Time.deltaTime);
        //    transform.Translate(horizontalMovement * horizontalInput * Time.deltaTime);
        //}

        if (this.isLocalPlayer)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if(horizontalInput < 0 && xPos > -xLimitRange || horizontalInput > 0 && xPos < xLimitRange)
            {
                transform.Translate(horizontalMovement * horizontalInput * Time.deltaTime);
            }
            if(verticalInput < 0 && zPos > lowerZLimitRange || verticalInput > 0 && zPos < upperZLimitRange)
            {
                transform.Translate(verticalMovement * verticalInput * Time.deltaTime);
            }
        }
    }
}
