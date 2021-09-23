using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class GameUI : MonoBehaviour
{
    public static GameUI gameUI;
    public GameObject titleMenu;
    public GameObject quitNotification;
    public GameObject settingMenu;
    public GameObject controlSettingMenu;
    public GameObject videoSettingMenu;
    public GameObject musicSettingMenu;
    public GameObject playGameMenu;
    public GameObject exitNotification;
    private Dropdown resolutionDropdown;
    private Slider mouseSensitiveSlider;
    private Slider musicVolumeSlider;
    private Slider SFXVolumeSlider;
    private TMP_InputField hostIP;
    public TMP_Text hostScore;
    public TMP_Text clientScore;
    public TMP_Text playTime;

    private int menuID;
    private int[,] resolution = { { 1920, 1080 }, { 1280, 720 }, { 800, 600 } };

    // Start is called before the first frame update
    void Start()
    {
        menuID = 1;
        gameUI = gameObject.GetComponent<GameUI>();
        resolutionDropdown = videoSettingMenu.gameObject.transform.Find("ResolutionDropdown").GetComponent<Dropdown>();
        mouseSensitiveSlider = controlSettingMenu.gameObject.transform.Find("Slider").GetComponent<Slider>();
        musicVolumeSlider = musicSettingMenu.gameObject.transform.Find("MusicSlider").GetComponent<Slider>();
        SFXVolumeSlider = musicSettingMenu.gameObject.transform.Find("SFXSlider").GetComponent<Slider>();
        hostIP = playGameMenu.gameObject.transform.Find("HostIp").GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Close game action
    /// </summary>
    public void CloseGame()
    {
        ShowNotification();
    }

    /// <summary>
    /// Show exit notification when quit button is clicked
    /// </summary>
    private void ShowNotification()
    {
        titleMenu.SetActive(false);
        quitNotification.SetActive(true);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Hide exit notification
    /// </summary>
    public void HideNotification()
    {
        titleMenu.SetActive(true);
        quitNotification.SetActive(false);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Call the Exit funtion in GameManager class
    /// </summary>
    public void Exit()
    {
        GameManager.gameManager.Exit();
    }

    /// <summary>
    /// Load setting menu
    /// </summary>
    public void LoadSettingMenu()
    {
        menuID++;
        titleMenu.SetActive(false);
        settingMenu.SetActive(true);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Load control setting menu
    /// </summary>
    public void LoadControlSettingMenu()
    {
        menuID++;
        settingMenu.SetActive(false);
        controlSettingMenu.SetActive(true);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Load video setting menu
    /// </summary>
    public void LoadVideoSettingMenu()
    {
        menuID++;
        videoSettingMenu.SetActive(true);
        settingMenu.SetActive(false);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Load music setting menu
    /// </summary>
    public void LoadMusicSettingMenu()
    {
        menuID++;
        musicSettingMenu.SetActive(true);
        settingMenu.SetActive(false);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Load play game menu
    /// </summary>
    public void LoadPlayGameMenu()
    {
        menuID++;
        playGameMenu.SetActive(true);
        titleMenu.SetActive(false);
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Back to previous menu
    /// </summary>
    public void Back()
    {
        switch (menuID)
        {
            case 3:
                controlSettingMenu.SetActive(false);
                videoSettingMenu.SetActive(false);
                musicSettingMenu.SetActive(false);
                settingMenu.SetActive(true);
                menuID--;
                break;
            case 2:
                settingMenu.SetActive(false);
                playGameMenu.SetActive(false);
                titleMenu.SetActive(true);
                menuID--;
                break;
            default:
                break;
        }
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Call TongleScreen Mode function in SettingManager class
    /// </summary>
    public void SetScreenMode()
    {
        SettingManager.settingManager.TongleScreenMode();
    }

    /// <summary>
    /// Call ChangeResolution function in SettingManager class
    /// </summary>
    public void SetResolution()
    {
        int index = resolutionDropdown.value;
        Debug.Log("Change resolution to: " + index);
        SettingManager.settingManager.ChangeResolution(resolution[index, 0], resolution[index, 1]);
    }

    /// <summary>
    /// Call ChangeMouseSensitive function in SettingManager class
    /// </summary>
    public void SetMouseSensitive()
    {
        SettingManager.settingManager.ChangeMouseSensitive(mouseSensitiveSlider.value);
    }

    /// <summary>
    /// Call ChangeMusicVolume function in SettingManager class
    /// </summary>
    public void SetMusicVolume()
    {
        SettingManager.settingManager.ChangeMusicVolume(musicVolumeSlider.value);
    }

    /// <summary>
    /// Call ChangeSFXVolume function in SettingManager class
    /// </summary>
    public void SetSFXVolume()
    {
        SettingManager.settingManager.ChangeSFXVolume(SFXVolumeSlider.value);
    }

    /// <summary>
    /// Load saved setting data to the UI
    /// </summary>
    /// <param name="mouseSensitive"> player movement speed when use mouse control </param>
    /// <param name="backgroundMusic"> the volume of background music played in the game </param>
    /// <param name="sfx"> the volume of sound effect played in the game </param>
    public void SetUISliderValue(float mouseSensitive, float backgroundMusic, float sfx)
    {
        mouseSensitiveSlider.value = mouseSensitive;
        musicVolumeSlider.value = backgroundMusic;
        SFXVolumeSlider.value = sfx;
    }

    /// <summary>
    /// Call OpenHost function in GameManager class
    /// </summary>
    public void OpenHost()
    {
        GameManager.gameManager.OpenHost();
        Debug.Log("Start host");
    }

    /// <summary>
    /// Call OpenClient function in GameManager class
    /// </summary>
    public void OpenClient()
    {
        string ip = hostIP.text;
        GameManager.gameManager.OpenClient(ip);
        Debug.Log("Connect to: " + ip);
        //GameManager.gameManager.LoadScene(0, hostIP.text);
    }

    /// <summary>
    /// Call PlayButtonClickSFX functin in SettingManager class
    /// </summary>
    private void PlayButtonClickSFX()
    {
        SettingManager.settingManager.playUISFX();
    }

    /// <summary>
    /// Set play score to the UI
    /// </summary>
    /// <param name="score">The score to be set</param>
    /// <param name="isHost">Is the score belong to host or client ?</param>
    public void SetScoreToUI(int score, bool isHost)
    {
        if (isHost)
        {
            hostScore.text = score.ToString();
        } else
        {
            clientScore.text = score.ToString();
        }
    }

    /// <summary>
    /// Display time remaining using mm:ss format
    /// </summary>
    /// <param name="minute"></param>
    /// <param name="seconds"></param>
    public void DisplayPlayTime(int minute, int seconds)
    {
        playTime.text = minute + ":" + seconds;
    }

    /// <summary>
    /// Show exit notification
    /// </summary>
    public void ShowExitNotification()
    {
        exitNotification.SetActive(true);
    }

    /// <summary>
    /// Hide exit notification
    /// </summary>
    public void HideExitNotification()
    {
        exitNotification.SetActive(false);
    }

    /// <summary>
    /// Exit the current game and return to title screen
    /// </summary>
    public void ExitGame()
    {
        GameManager.gameManager.ReturnToTitleMenu();
    }
}