using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Referee : NetworkBehaviour
{
    public static Referee referee;
    public GameObject iceHockeyBall;
    private Vector3 startPos;

    private int hostScore, clientScore;
    [SerializeField]
    private int minute, second, countFromThree;
    [SerializeField]
    private int playerReady;

    // Start is called before the first frame update
    void Start()
    {
        referee = gameObject.GetComponent<Referee>();
        InitializeValues();
        SpawnHockeyBall();
    }

    private void InitializeValues()
    {
        playerReady = 0;
        minute = 5;
        second = 0;
        countFromThree = 3;
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
    }

    /// <summary>
    /// Calculate the time remain
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDown()
    {
        Debug.Log("Ready to play");
        PlayerController.player.RpcHideAfterWaiting();
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
            second--;
            if (second < 0)
            {
                second = 59;
                minute--;
            }
            if (minute == 0 && second == 0)
            {
                break;
            }
            UpdateToInstances("play time");
        }
    }

    /// <summary>
    /// Update playtime to all instances
    /// </summary>
    private void UpdateToInstances(string opt)
    {
        switch (opt)
        {
            case "play time":
                PlayerController.player.RpcDisplayPlayTime(minute, second);
                break;
            case "start countdown":
                PlayerController.player.RpcDisplayCountDown(countFromThree, true);
                break;
            case "hide countdown":
                PlayerController.player.RpcDisplayCountDown(countFromThree, false);
                break;
            case "set active":
                PlayerController.player.RpcSetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetReady()
    {
        playerReady++;
        if(playerReady == 1)
        {
            StartGame();
        }
    }

    [Server]
    public void StartGame()
    {
        StartCoroutine(CountDown());
    }
}
