using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Referee : NetworkBehaviour
{
    public static Referee referee;
    public GameObject iceHockeyBall;
    public GameObject hostNetworkSpawnPos;
    private GameObject hostObject;
    private Rigidbody hostObjectRigid;
    private Vector3 startPos;
    private Coroutine countdownCoroutine;

    private int hostScore, clientScore;
    [SerializeField]
    private int defaulMinute, defaultSeconds, currentMinute, currentSeconds, countedMinute, countedSeconds, countFromThree;
    [SerializeField]
    private int playerReady;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        referee = gameObject.GetComponent<Referee>();
        InitializeValues();
        SpawnHockeyBall();
    }

    /// <summary>
    /// Initiate the importants values
    /// </summary>
    private void InitializeValues()
    {
        playerReady = 0;
        defaulMinute = 10;
        defaultSeconds = 0;
        hostScore = clientScore = 0;
        startPos = new Vector3(0, 0.149f, 0);
        hostObject = null;
        hostObjectRigid = null;
        Debug.Log("Initialize successfully");
    }

    /// <summary>
    /// Spawn hockey ball
    /// </summary>
    private void SpawnHockeyBall()
    {
        GameObject ball = Instantiate(iceHockeyBall, startPos, Quaternion.identity);
        NetworkServer.Spawn(ball);
    }

    /// <summary>
    /// Caculate the score and display to UI
    /// </summary>
    /// <param name="isHost">True if the caculated score is belong to host and false if it's belong to client</param>
    public void AddScore(bool isHost)
    {
        if (isHost)
        {
            hostScore++;
            PlayerController.player.RpcUpdateToUI(hostScore, isHost);
        }
        else
        {
            clientScore++;
            PlayerController.player.RpcUpdateToUI(clientScore, isHost);
        }
        StopCounting();
        StartCoroutine(GoalProcedure());
    }

    /// <summary>
    /// Calculate the time remain
    /// </summary>
    /// <param name="fromMinute">the minute value that is counted from</param>
    /// <param name="fromSeconds">the seconds value that is counted from<</param>
    /// <returns>Return the delayed time (1 seconds)</returns>
    IEnumerator CountDown(int fromMinute, int fromSeconds)
    {
        countedMinute = fromMinute;
        countedSeconds = fromSeconds;
        countFromThree = 3;
        Debug.Log("Ready to play from: " + fromMinute + ":" + fromSeconds);
        PlayerController.player.RpcHideAfterWaiting();
        PlayerController.player.RpcDisplayGoalText(false, "not thing");
        PlayerController.player.RpcSetDefaultPos();
        SetDefaultHostObject();
        IceHockeyBallBehaviour.behav.RpcSetDefaultPos();

        while (countFromThree >= 0)
        {
            UpdateToInstances("start countdown");
            countFromThree--;
            yield return new WaitForSeconds(1);
        }

        UpdateToInstances("hide countdown");
        UpdateToInstances("set active");

        while (true)
        {
            yield return new WaitForSeconds(1);
            countedSeconds--;
            if (countedSeconds < 0)
            {
                countedSeconds = 59;
                countedMinute--;
            }
            UpdateToInstances("play time");
            if (countedMinute == 0 && countedSeconds == 0)
            {
                break;
            }
        }
        TimeSUp();
        Debug.Log("Time's up !!!");
    }

    /// <summary>
    /// Called when a player get a score
    /// </summary>
    /// <returns>Return the delayed time (1.5 seconds)</returns>
    IEnumerator GoalProcedure()
    {
        PlayerController.player.RpcDisplayGoalText(true, "Goal !!!");
        PlayerController.player.RpcSetActive(false);
        yield return new WaitForSeconds(1.5f);
        countdownCoroutine = StartCoroutine(CountDown(currentMinute, currentSeconds));
    }

    /// <summary>
    /// Update infomation to the UI of all instances
    /// </summary>
    private void UpdateToInstances(string opt)
    {
        switch (opt)
        {
            case "play time":
                PlayerController.player.RpcDisplayPlayTime(countedMinute, countedSeconds);
                break;
            case "start countdown":
                PlayerController.player.RpcDisplayCountDown(countFromThree, true);
                break;
            case "hide countdown":
                PlayerController.player.RpcDisplayCountDown(countFromThree, false);
                break;
            case "set active":
                PlayerController.player.RpcSetActive(true);
                IceHockeyBallBehaviour.behav.RpcSetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Change the game from waiting state to ready state
    /// </summary>
    public void SetReady()
    {
        playerReady++;
        if (playerReady == 1)
        {
            StartGame();
        }
    }

    /// <summary>
    /// Stop the countdown coroutine and save current minute and seconds
    /// </summary>
    private void StopCounting()
    {
        currentMinute = countedMinute;
        currentSeconds = countedSeconds;
        StopCoroutine(countdownCoroutine);
        countdownCoroutine = null;
    }

    /// <summary>
    /// Start the game
    /// </summary>
    [Server]
    public void StartGame()
    {
        countdownCoroutine = StartCoroutine(CountDown(defaulMinute, defaultSeconds));
    }

    /// <summary>
    /// Calculate the winner
    /// </summary>
    private void TimeSUp()
    {
        bool isHostWin = (hostScore - clientScore) > 0 ? true : false;
        PlayerController.player.RpcDisplayGoalText(true, "Time's up");
        PlayerController.player.RpcSetActive(false);
        IceHockeyBallBehaviour.behav.RpcSetActive(false);
        PlayerController.player.WinnerAnnoucement(true, isHostWin);
    }

    /// <summary>
    /// Place local client player gameobject to start position
    /// </summary>
    private void SetDefaultHostObject()
    {
        if (hostObject == null || hostObjectRigid == null)
        {
            hostObject = GameObject.Find("Player [connId=0]");
            hostObjectRigid = hostObject.GetComponent<Rigidbody>();
        }
        hostObject.SetActive(false);
        hostObject.transform.position = hostNetworkSpawnPos.transform.position;
        hostObjectRigid.velocity = new Vector3(0, 0, 0);
        hostObject.SetActive(true);
    }
}
