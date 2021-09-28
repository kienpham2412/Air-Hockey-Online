using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Referee : NetworkBehaviour
{
    public static Referee referee;
    public GameObject iceHockeyBall;
    private Vector3 startPos;
    private Coroutine countdownCoroutine;

    private int hostScore, clientScore;
    [SerializeField]
    private int defaulMinute, defaultSeconds, currentMinute, currentSeconds, countedMinute, countedSeconds, countFromThree;
    [SerializeField]
    private int playerReady;

    // Start is called before the first frame update
    void Start()
    {
        referee = gameObject.GetComponent<Referee>();
        InitializeValues();
        SpawnHockeyBall();
    }

    /// <summary>
    /// Initiate some values
    /// </summary>
    private void InitializeValues()
    {
        playerReady = 0;
        defaulMinute = 0;
        defaultSeconds = 10;
        hostScore = clientScore = 0;
        startPos = new Vector3(0, 0.149f, 0);
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
    /// <param name="isHost"></param>
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
    /// <returns></returns>
    IEnumerator CountDown(int fromMinute, int fromSeconds)
    {
        countedMinute = fromMinute;
        countedSeconds = fromSeconds;
        countFromThree = 3;
        Debug.Log("Ready to play from: " + fromMinute + ":" + fromSeconds);
        PlayerController.player.RpcHideAfterWaiting();
        PlayerController.player.RpcDisplayGoalText(false, "not thing");
        PlayerController.player.RpcSetDefaultPos();
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
    /// <returns></returns>
    IEnumerator GoalProcedure()
    {
        PlayerController.player.RpcDisplayGoalText(true, "Goal !!!");
        PlayerController.player.RpcSetActive(false);
        yield return new WaitForSeconds(1.5f);
        countdownCoroutine = StartCoroutine(CountDown(currentMinute, currentSeconds));
    }

    /// <summary>
    /// Update playtime to all instances
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
        if(playerReady == 1)
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
}
