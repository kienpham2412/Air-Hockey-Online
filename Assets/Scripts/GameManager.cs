using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    /// <summary>
    /// Start new scene as host
    /// </summary>
    public void OpenHost()
    {
        PlayerPrefs.SetInt("IsHost", 1);
        PlayerPrefs.SetString("HostIP", " ");
        SceneManager.LoadSceneAsync("Gameplay");
    }

    /// <summary>
    /// Start new scene as client
    /// </summary>
    /// <param name="ip">The host's ip to connect to</param>
    public void OpenClient(string ip)
    {
        PlayerPrefs.SetInt("IsHost", 0);
        PlayerPrefs.SetString("HostIP", ip);
        SceneManager.LoadSceneAsync("Gameplay");
    }

    /// <summary>
    /// Exit the match and return to title menu
    /// </summary>
    public void ReturnToTitleMenu()
    {
        NetworkConnector.connector.CloseConnection();
        SceneManager.LoadSceneAsync("TitleScreen");
    }

    /// <summary>
    /// Exit game and close the window
    /// </summary>
    public void Exit()
    {
#if UNITY_EDITOR    // preprocessor directive 
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
