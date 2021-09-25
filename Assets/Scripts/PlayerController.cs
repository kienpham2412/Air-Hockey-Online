using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController player;
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
    [SerializeField]
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        player = gameObject.GetComponent<PlayerController>();
        speed = PlayerPrefs.GetFloat("MouseSensitive");
        verticalMovement = Vector3.forward * speed;
        horizontalMovement = Vector3.right * speed;
        if (this.isServer)
        {
            upperZLimitRange = middleBorder;
            lowerZLimitRange = -zLimitRange;
            GameUI.gameUI.DisplayInWaiting(true, true);
        }
        else
        {
            upperZLimitRange = zLimitRange;
            lowerZLimitRange = middleBorder;
            GameUI.gameUI.DisplayInWaiting(false, true);
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

        if (this.isLocalPlayer && active)
        {
            //horizontalInput = Input.GetAxis("Mouse X");
            //verticalInput = Input.GetAxis("Mouse Y");
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

    [Command]
    public void CmdSetReady()
    {
        Referee.referee.SetReady();
    }

    [ClientRpc]
    public void RpcUpdateToUI(int score, bool isHost)
    {
        GameUI.gameUI.SetScoreToUI(score, isHost);
    }

    [ClientRpc]
    public void RpcHideAfterWaiting()
    {
        GameUI.gameUI.DisplayInWaiting(false, false);
        GameUI.gameUI.DisplayInWaiting(true, false);
    }

    [ClientRpc]
    public void RpcDisplayPlayTime(int minute, int second)
    {
        GameUI.gameUI.DisplayPlayTime(minute, second);
    }

    [ClientRpc]
    public void RpcDisplayCountDown(int num, bool isActive)
    {
        GameUI.gameUI.DisplayCountDown(num, isActive);
    }

    [ClientRpc]
    public void RpcSetActive(bool active)
    {
        this.active = active;
    }
}
