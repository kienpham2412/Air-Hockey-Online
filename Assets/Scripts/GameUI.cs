using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public GameObject waitingNotification;
    public GameObject readyButton;
    public GameObject countDown;
    public GameObject goalText;
    private Dropdown resolutionDropdown;
    private Toggle fullScreenToggle;
    private Slider mouseSensitiveSlider;
    private Slider musicVolumeSlider;
    private Slider SFXVolumeSlider;
    private TMP_InputField hostIP;
    public TMP_Text hostScore;
    public TMP_Text clientScore;
    public TMP_Text playTime;
    public TMP_Text countDownText;
    public Scene currentScene;

    private int menuID;
    private int[,] resolution = { { 1920, 1080 }, { 1280, 720 }, { 800, 600 } };

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        menuID = 1;
        gameUI = gameObject.GetComponent<GameUI>();
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "TitleScreen")
        {
            fullScreenToggle = videoSettingMenu.gameObject.transform.Find("FullScreenToggle").GetComponent<Toggle>();
            resolutionDropdown = videoSettingMenu.gameObject.transform.Find("ResolutionDropdown").GetComponent<Dropdown>();
            mouseSensitiveSlider = controlSettingMenu.gameObject.transform.Find("Slider").GetComponent<Slider>();
            musicVolumeSlider = musicSettingMenu.gameObject.transform.Find("MusicSlider").GetComponent<Slider>();
            SFXVolumeSlider = musicSettingMenu.gameObject.transform.Find("SFXSlider").GetComponent<Slider>();
            hostIP = playGameMenu.gameObject.transform.Find("HostIp").GetComponent<TMP_InputField>();
            LoadSettingData();
        }
    }

    /// <summary>
    /// Start the close procedure
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
    /// Start the exit procedure
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

    //public void LoadControlSettingMenu()
    //{
    //    menuID++;
    //    settingMenu.SetActive(false);
    //    controlSettingMenu.SetActive(true);
    //    PlayButtonClickSFX();
    //}

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
    /// Load choose game mode menu
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
                SettingManager.settingManager.SaveSettingData(true);
                menuID--;
                break;
            default:
                break;
        }
        PlayButtonClickSFX();
    }

    /// <summary>
    /// Receive player's interacion and start the toggle fullscreen procedure
    /// </summary>
    public void SetScreenMode()
    {
        SettingManager.settingManager.TongleScreenMode();
    }

    /// <summary>
    /// Receive player's interacion and start the change resolution procedure
    /// </summary>
    public void SetResolution()
    {
        int index = resolutionDropdown.value;
        Debug.Log("Change resolution to: " + index);
        SettingManager.settingManager.ChangeResolution(index, resolution[index, 0], resolution[index, 1]);
    }

    //public void SetMouseSensitive()
    //{
    //    SettingManager.settingManager.ChangeMouseSensitive(mouseSensitiveSlider.value);
    //}

    /// <summary>
    /// Receive player's interacion and start the change music volume procedure
    /// </summary>
    public void SetMusicVolume()
    {
        SettingManager.settingManager.ChangeMusicVolume(musicVolumeSlider.value);
    }

    /// <summary>
    /// Receive player's interacion and start the change sfx volume procedure
    /// </summary>
    public void SetSFXVolume()
    {
        SettingManager.settingManager.ChangeSFXVolume(SFXVolumeSlider.value);
    }

    /// <summary>
    /// Load saved setting data to the UI
    /// </summary>
    /// <param name="backgroundMusic">The volume of background music played in the game </param>
    /// <param name="sfx">The volume of sound effect played in the game </param>
    public void SetUISliderValue(float backgroundMusic, float sfx)
    {
        musicVolumeSlider.value = backgroundMusic;
        SFXVolumeSlider.value = sfx;
    }

    /// <summary>
    /// Receive player's interacion and open a new scene as host
    /// </summary>
    public void OpenHost()
    {
        GameManager.gameManager.OpenHost();
        Debug.Log("Start host");
    }

    /// <summary>
    /// Receive player's interacion and open a new scene as client
    /// </summary>
    public void OpenClient()
    {
        string ip = hostIP.text;
        GameManager.gameManager.OpenClient(ip);
        Debug.Log("Connect to: " + ip);
        //GameManager.gameManager.LoadScene(0, hostIP.text);
    }

    /// <summary>
    /// Call the play sfx procedure when player click a button
    /// </summary>
    private void PlayButtonClickSFX()
    {
        SettingManager.settingManager.playUISFX();
    }

    /// <summary>
    /// Display the player score to the UI
    /// </summary>
    /// <param name="score">The score to be set</param>
    /// <param name="isHost">Is the score belong to host or client ?</param>
    public void SetScoreToUI(int score, bool isHost)
    {
        if (isHost)
        {
            hostScore.text = score.ToString();
        }
        else
        {
            clientScore.text = score.ToString();
        }
    }

    /// <summary>
    /// Display remaining time using mm:ss format
    /// </summary>
    /// <param name="minute">Remaining minutes</param>
    /// <param name="seconds">Remaining seconds</param>
    public void DisplayPlayTime(int minute, int seconds)
    {
        string minuteString, secondsString;
        if (minute < 10)
        {
            minuteString = "0" + minute;
        }
        else
        {
            minuteString = minute.ToString();
        }
        if (seconds < 10)
        {
            secondsString = "0" + seconds;
        }
        else
        {
            secondsString = seconds.ToString();
        }
        playTime.text = minuteString + ":" + secondsString;
    }

    /// <summary>
    /// Show exit notification
    /// </summary>
    public void ShowExitNotification()
    {
        exitNotification.SetActive(true);
        SettingManager.settingManager.playUISFX();
    }

    /// <summary>
    /// Hide exit notification
    /// </summary>
    public void HideExitNotification()
    {
        exitNotification.SetActive(false);
        SettingManager.settingManager.playUISFX();
    }

    /// <summary>
    /// Exit the current game and return to title screen
    /// </summary>
    public void ExitGame()
    {
        SettingManager.settingManager.playUISFX();
        GameManager.gameManager.ReturnToTitleMenu();
    }

    /// <summary>
    /// Display and hide waiting notification and ready button
    /// </summary>
    /// <param name="isWaiting">True if waiting notification is going to be shown and false if ready button is going to be shown</param>
    /// <param name="isShown">True if the UI is going to be shown and false if it's going to be hided</param>
    public void DisplayInWaiting(bool isWaiting, bool isShown)
    {
        if (isWaiting)
        {
            waitingNotification.SetActive(isShown);
        }
        else
        {
            readyButton.SetActive(isShown);
        }
    }

    /// <summary>
    /// Display the countdown UI
    /// </summary>
    /// <param name="countDownNum">The value that is going to be shown</param>
    /// <param name="isActive">Is the UI going to be shown or hided ? (true, false)</param>
    public void DisplayCountDown(int countDownNum, bool isActive)
    {
        countDown.SetActive(isActive);
        if (countDownNum == 0)
        {
            countDownText.text = "Ready";
        }
        else
        {
            countDownText.text = countDownNum.ToString();
        }
    }

    /// <summary>
    /// Start the ready procedure from server
    /// </summary>
    public void SetReady()
    {
        PlayerController.player.CmdSetReady();
        readyButton.transform.Find("ReadyButton").GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// Display the goal text when a player get a socre
    /// </summary>
    /// <param name="isDisplay">Is the text being shown or hided</param>
    public void DisplayGoalText(bool isDisplay, string content)
    {
        goalText.GetComponent<TextMeshProUGUI>().text = content;
        goalText.SetActive(isDisplay);
    }

    /// <summary>
    /// Load saved setting data to UI
    /// </summary>
    private void LoadSettingData()
    {
        SettingData setting = SettingManager.settingManager.GetSavedData();
        fullScreenToggle.isOn = setting.fullScreen;
        resolutionDropdown.value = setting.resolutionIndex;
        musicVolumeSlider.value = setting.musicVolume;
        SFXVolumeSlider.value = setting.SFXVolume;
        Debug.Log("Update data to UI successfully");
    }
}
