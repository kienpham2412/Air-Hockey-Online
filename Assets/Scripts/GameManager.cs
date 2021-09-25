using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    /// <summary>
    /// Create a host instance using Mirror
    /// </summary>
    public void OpenHost()
    {
        PlayerPrefs.SetInt("IsHost", 1);
        PlayerPrefs.SetString("HostIP", " ");
        SceneManager.LoadSceneAsync("Gameplay");
    }

    /// <summary>
    /// Create a client instance using Mirror
    /// </summary>
    /// <param name="ip"></param>
    public void OpenClient(string ip)
    {
        PlayerPrefs.SetInt("IsHost", 0);
        PlayerPrefs.SetString("HostIP", ip);
        SceneManager.LoadSceneAsync("Gameplay");
    }

    /// <summary>
    /// Return to title menu
    /// </summary>
    public void ReturnToTitleMenu()
    {
        NetworkConnector.connector.CloseConnection();
        SceneManager.LoadSceneAsync("TitleScreen");
    }

    /// <summary>
    /// Exit to desktop
    /// </summary>
    public void Exit()
    {
#if UNITY_EDITOR    // preprocessor directive 
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
