using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Referee : NetworkBehaviour
{
    public static Referee referee;
    public GameObject iceHockeyBall;

    [SyncVar]
    private int hostScore, clientScore;
    [SyncVar]
    private int minute, second;
    // Start is called before the first frame update
    void Start()
    {
        referee = gameObject.GetComponent<Referee>();
        hostScore = clientScore = 0;
        
        if (this.isServer)
        {
            minute = 5;
            second = 0;
            StartCoroutine(CountDown());
        }
    }

    /// <summary>
    /// Spawn hockey ball
    /// </summary>
    private void SpawnHockeyBall()
    {
        iceHockeyBall.SetActive(true);
    }

    /// <summary>
    /// Caculate the score and display to UI
    /// </summary>
    /// <param name="isHost"></param>
    [ClientRpc]
    public void RpcAddScore(bool isHost)
    {
        if (isHost)
        {
            hostScore++;
            GameUI.gameUI.SetScoreToUI(hostScore, isHost);
        }
        else
        {
            clientScore++;
            GameUI.gameUI.SetScoreToUI(clientScore, isHost);
        }
    }

    /// <summary>
    /// Calculate the time remain
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDown()
    {
        while (minute >= 0)
        {
            yield return new WaitForSeconds(1);
            second--;
            if (second < 0)
            {
                second = 59;
                minute--;
            }
            if (minute == 0 && second == 0) break;
            RcpUpdateToInstances();
        }
    }

    /// <summary>
    /// Call DisplayPlayTime function in GameUI class to display time
    /// </summary>
    [ClientRpc]
    private void RcpUpdateToInstances()
    {
        GameUI.gameUI.DisplayPlayTime(minute, second);
    }
}
