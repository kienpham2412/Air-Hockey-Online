using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController player;
    private Vector3 verticalMovement;
    private Vector3 horizontalMovement;
    private Vector3 playerStartPos;
    private Rigidbody playerRigidBody;

    [SerializeField] private float speed = 10;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    private float xPos, zPos;
    private float xLimitRange = 0.4f;
    private float zLimitRange = 0.75f;
    private float middleBorder = 0f;
    private float upperZLimitRange, lowerZLimitRange;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
        playerRigidBody = gameObject.GetComponent<Rigidbody>();

        verticalMovement = Vector3.forward * speed;
        horizontalMovement = Vector3.right * speed;
        playerStartPos = gameObject.transform.position;
        playerRigidBody.velocity = new Vector3(0, 0, 0);

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

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
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

        if (this.isLocalPlayer && SettingManager.settingManager.playerActive)
        {
            //horizontalInput = Input.GetAxis("Mouse X");
            //verticalInput = Input.GetAxis("Mouse Y");
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (horizontalInput < 0 && xPos > -xLimitRange || horizontalInput > 0 && xPos < xLimitRange)
            {
                transform.Translate(horizontalMovement * horizontalInput * Time.deltaTime);
            }
            if (verticalInput < 0 && zPos > lowerZLimitRange || verticalInput > 0 && zPos < upperZLimitRange)
            {
                transform.Translate(verticalMovement * verticalInput * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Start the ready procedure
    /// </summary>
    [Command]
    public void CmdSetReady()
    {
        Referee.referee.SetReady();
    }

    /// <summary>
    /// Update score to the UI
    /// </summary>
    /// <param name="score">The score value to be updated</param>
    /// <param name="isHost">Is the score belong to host or client</param>
    [ClientRpc]
    public void RpcUpdateToUI(int score, bool isHost)
    {
        GameUI.gameUI.SetScoreToUI(score, isHost);
    }

    /// <summary>
    /// This is a kind of message that is invoked to hide the waiting notification and ready button in the UI
    /// </summary>
    [ClientRpc]
    public void RpcHideAfterWaiting()
    {
        GameUI.gameUI.DisplayInWaiting(false, false);
        GameUI.gameUI.DisplayInWaiting(true, false);
    }

    /// <summary>
    /// This is a kind of message that is invoked to display the remaining time of a round in the UI
    /// </summary>
    /// <param name="minute">Remaining minutes</param>
    /// <param name="second">Remaining seconds</param>
    [ClientRpc]
    public void RpcDisplayPlayTime(int minute, int second)
    {
        GameUI.gameUI.DisplayPlayTime(minute, second);
    }

    /// <summary>
    /// This is a kind of message that is invoked to display the countdown at the beginning of a round in the UI
    /// </summary>
    /// <param name="num"></param>
    /// <param name="isActive"></param>
    [ClientRpc]
    public void RpcDisplayCountDown(int num, bool isActive)
    {
        GameUI.gameUI.DisplayCountDown(num, isActive);
    }

    /// <summary>
    /// Set controllability to the player
    /// </summary>
    /// <param name="isActive">Is the player gameobject controllable ?</param>
    [ClientRpc]
    public void RpcSetActive(bool isActive)
    {
        SettingManager.settingManager.playerActive = isActive;
    }

    /// <summary>
    /// Place the player at start postiton
    /// </summary>
    [ClientRpc]
    public void RpcSetDefaultPos()
    {
        this.gameObject.SetActive(false);
        this.transform.position = this.playerStartPos;
        this.playerRigidBody.velocity = new Vector3(0, 0, 0);
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// This is a kind of message that is invoked to display the goal text in the UI
    /// </summary>
    /// <param name="isDisplay">Is this text being shown? (true or false)</param>
    /// <param name="content">The content that is being shown</param>
    [ClientRpc]
    public void RpcDisplayGoalText(bool isDisplay, string content)
    {
        GameUI.gameUI.DisplayGoalText(isDisplay, content);
    }

    /// <summary>
    /// Announce who is the winner
    /// </summary>
    /// <param name="isDisplay">Hide or display this announcement</param>
    /// <param name="isHostWin">True if the host win and false if the client win</param>
    [ClientRpc]
    public void WinnerAnnoucement(bool isDisplay, bool isHostWin)
    {
        if (this.isServer)
        {
            if (isHostWin)
            {
                GameUI.gameUI.DisplayGoalText(isDisplay, "You win");
                SettingManager.settingManager.playEndgameSFX(true);
            }
            else
            {
                GameUI.gameUI.DisplayGoalText(isDisplay, "You lose");
                SettingManager.settingManager.playEndgameSFX(false);
            }
        }
        else
        {
            if (isHostWin)
            {
                GameUI.gameUI.DisplayGoalText(isDisplay, "You lose");
                SettingManager.settingManager.playEndgameSFX(false);
            }
            else
            {
                GameUI.gameUI.DisplayGoalText(isDisplay, "You win");
                SettingManager.settingManager.playEndgameSFX(true);
            }
        }
    }

    /// <summary>
    /// Play sound effect
    /// </summary>
    /// <param name="sfxName">Name of the sound effect</param>
    [ClientRpc]
    public void RpcPlaySFX(string sfxName)
    {
        switch (sfxName)
        {
            case "Gameplay":
                SettingManager.settingManager.playGameSFX();
                break;
            case "Goal":
                SettingManager.settingManager.playGoalSFX();
                break;
            case "Win":
                SettingManager.settingManager.playEndgameSFX(true);
                break;
            case "Lose":
                SettingManager.settingManager.playEndgameSFX(false);
                break;
            default:
                break;
        }
    }
}
